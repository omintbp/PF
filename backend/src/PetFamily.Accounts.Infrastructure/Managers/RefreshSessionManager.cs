using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.Managers;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Accounts.Infrastructure.DbContexts.Write;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.Managers;

public class RefreshSessionManager : IRefreshSessionManager
{
    private const string REFRESH_TOKEN = "refreshToken";

    private readonly AccountWriteDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RefreshSessionManager(AccountWriteDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshToken,
        CancellationToken cancellationToken = default)
    {
        var result = await _context.RefreshSessions
            .Include(r => r.User)
            .ThenInclude(u => u.Roles)
            .FirstOrDefaultAsync(r => r.RefreshToken == refreshToken, cancellationToken);

        if (result == null)
            return Errors.General.NotFound(refreshToken);

        return result;
    }

    public void Delete(RefreshSession refreshSession)
    {
        _context.RefreshSessions.Remove(refreshSession);
    }

    public Result<Guid, Error> GetRefreshSessionCookie()
    {
        if (_httpContextAccessor.HttpContext is null)
            return Errors.User.HttpContextUnavailable();

        if (!_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(REFRESH_TOKEN, out var refreshToken))
            return Errors.User.RefreshTokenNotFound();

        return Guid.Parse(refreshToken);
    }

    public UnitResult<Error> SetRefreshSessionCookie(Guid refreshToken)
    {
        if (_httpContextAccessor.HttpContext is null)
            return Errors.User.HttpContextUnavailable();

        _httpContextAccessor.HttpContext.Response.Cookies.Append(REFRESH_TOKEN, refreshToken.ToString());

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> DeleteRefreshSessionCookie()
    {
        if (_httpContextAccessor.HttpContext is null)
            return Errors.User.HttpContextUnavailable();

        _httpContextAccessor.HttpContext.Response.Cookies.Delete(REFRESH_TOKEN);

        return UnitResult.Success<Error>();
    }
}