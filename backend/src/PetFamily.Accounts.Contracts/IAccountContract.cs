using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Contracts;

public interface IAccountContract
{
    public Result<Guid, Error> GetCurrentUserId(HttpContext context);

    public Task<bool> CheckIfUserHasPermission(
        Guid userId,
        string permission,
        CancellationToken cancellation = default);

    public Task<Result<Guid, ErrorList>> CreateVolunteerAccount(
        Guid userId,
        Experience experience,
        List<Requisite> requisites,
        CancellationToken cancellation = default);
}