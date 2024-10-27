using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Accounts.Application.Managers;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Queries.CheckIfUserHasPermission;

public class CheckIfUserHasPermissionQueryHandler
    : IQueryHandler<bool, CheckIfUserHasPermissionQuery>
{
    private readonly IPermissionManager _permissionManager;
    private readonly IValidator<CheckIfUserHasPermissionQuery> _validator;

    public CheckIfUserHasPermissionQueryHandler(
        IPermissionManager permissionManager,
        IValidator<CheckIfUserHasPermissionQuery> validator)
    {
        _permissionManager = permissionManager;
        _validator = validator;
    }

    public async Task<Result<bool, ErrorList>> Handle(
        CheckIfUserHasPermissionQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var isUserHasPermission = await _permissionManager.CheckIfUserHasPermission(
            query.UserId,
            query.Permission,
            cancellationToken);

        return isUserHasPermission;
    }
}