using System.Security.Claims;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using PetFamily.Accounts.Application.Queries.CheckIfUserHasPermission;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Presentation;

public class AccountContract : IAccountContract
{
    private readonly IQueryHandler<bool, CheckIfUserHasPermissionQuery> _checkIfUserHasPermissionHandler;

    public AccountContract(IQueryHandler<bool, CheckIfUserHasPermissionQuery> checkIfUserHasPermissionHandler)
    {
        _checkIfUserHasPermissionHandler = checkIfUserHasPermissionHandler;
    }

    public Result<Guid, Error> GetCurrentUserId(HttpContext context)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.Id);
        if (userIdClaim == null)
            return Errors.General.NotFound();

        if (Guid.TryParse(userIdClaim.Value, out var userId))
            return userId;

        return Errors.General.NotFound();
    }

    public async Task<bool> CheckIfUserHasPermission(
        Guid userId,
        string permission,
        CancellationToken cancellationToken)
    {
        var query = new CheckIfUserHasPermissionQuery(userId, permission);

        var result = await _checkIfUserHasPermissionHandler.Handle(query, cancellationToken);
        
        if(result.IsFailure)
            return false;

        return result.Value;
    }
}