using PetFamily.Accounts.Application.Queries.CheckIfUserHasPermission;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Presentation;

public class AccountContract : IAccountContract
{
    private readonly IQueryHandler<bool, CheckIfUserHasPermissionQuery> _checkIfUserHasPermissionHandler;

    public AccountContract(IQueryHandler<bool, CheckIfUserHasPermissionQuery> checkIfUserHasPermissionHandler)
    {
        _checkIfUserHasPermissionHandler = checkIfUserHasPermissionHandler;
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