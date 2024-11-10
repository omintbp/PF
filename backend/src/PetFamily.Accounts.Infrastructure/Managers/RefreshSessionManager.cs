using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.Managers;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.Managers;

public class RefreshSessionManager : IRefreshSessionManager
{
    private readonly AuthorizationDbContext _context;

    public RefreshSessionManager(AuthorizationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshToken,
        CancellationToken cancellationToken = default)
    {
        var result = await _context.RefreshSessions
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.RefreshToken == refreshToken, cancellationToken);

        if (result == null)
            return Errors.General.NotFound(refreshToken);

        return result;
    }

    public void Delete(RefreshSession refreshSession)
    {
        _context.RefreshSessions.Remove(refreshSession);
    }
}