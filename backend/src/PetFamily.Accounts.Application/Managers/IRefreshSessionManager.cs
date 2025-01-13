using CSharpFunctionalExtensions;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Managers;

public interface IRefreshSessionManager
{
    Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshToken,
        CancellationToken cancellationToken = default);

    void Delete(RefreshSession refreshSession);

    Result<Guid, Error> GetRefreshSessionCookie();

    public UnitResult<Error> SetRefreshSessionCookie(Guid refreshToken);

    public UnitResult<Error> DeleteRefreshSessionCookie();
}