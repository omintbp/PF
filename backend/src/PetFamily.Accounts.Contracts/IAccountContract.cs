using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Contracts;

public interface IAccountContract
{
    public Result<Guid, Error> GetCurrentUserId(HttpContext context);

    public Task<bool> CheckIfUserHasPermission(
        Guid userId,
        string permission,
        CancellationToken cancellation = default);
}