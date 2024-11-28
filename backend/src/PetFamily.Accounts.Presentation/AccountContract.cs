using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using PetFamily.Accounts.Application.Commands.CreateVolunteerAccount;
using PetFamily.Accounts.Application.Queries.CheckIfUserHasPermission;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs.Shared;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Presentation;

public class AccountContract : IAccountContract
{
    private readonly IQueryHandler<bool, CheckIfUserHasPermissionQuery> _checkIfUserHasPermissionHandler;
    private readonly ICommandHandler<Guid, CreateVolunteerAccountCommand> _createVolunteerAccountHandler;

    public AccountContract(
        ICommandHandler<Guid, CreateVolunteerAccountCommand> createVolunteerAccountHandler,
        IQueryHandler<bool, CheckIfUserHasPermissionQuery> checkIfUserHasPermissionHandler)
    {
        _createVolunteerAccountHandler = createVolunteerAccountHandler;
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

        if (result.IsFailure)
            return false;

        return result.Value;
    }

    public async Task<Result<Guid, ErrorList>> CreateVolunteerAccount(
        Guid userId,
        Experience experience,
        List<Requisite> requisites,
        CancellationToken cancellation = default)
    {
        var requisitesDto = requisites.Select(r =>
            new RequisiteDto(r.Name, r.Description));

        var command = new CreateVolunteerAccountCommand(userId, experience.Value, requisitesDto);

        var result = await _createVolunteerAccountHandler.Handle(command, cancellation);

        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }
}