using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Framework.Extensions;
using PetFamily.VolunteerRequests.Application.Commands.CreateVolunteerRequest;
using PetFamily.VolunteerRequests.Application.Commands.SendVolunteerRequestToRevision;
using PetFamily.VolunteerRequests.Application.Commands.TakeVolunteerRequestToReview;
using PetFamily.VolunteerRequests.Contracts.Requests;

namespace PetFamily.VolunteerRequests.Presentation;

public class VolunteerRequestsController : ApplicationController
{
    [HttpPost("create")]
    [Permission(Permissions.VolunteerRequests.CreateVolunteerRequest)]
    public async Task<ActionResult> CreateVolunteerRequest(
        [FromBody] CreateVolunteerRequestRequest request,
        [FromServices] ICommandHandler<Guid, CreateVolunteerRequestCommand> handler,
        [FromServices] IAccountContract accountContract,
        CancellationToken cancellationToken = default)
    {
        var userIdResult = accountContract.GetCurrentUserId(HttpContext);
        if (userIdResult.IsFailure)
            return userIdResult.Error.ToResponse();

        var command = new CreateVolunteerRequestCommand(
            userIdResult.Value,
            request.Experience,
            request.Requisites);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{volunteerRequestId::guid}/take-to-review")]
    [Permission(Permissions.VolunteerRequests.TakeVolunteerRequestToReview)]
    public async Task<ActionResult> TakeVolunteerRequestToReview(
        [FromRoute] Guid volunteerRequestId,
        [FromServices] ICommandHandler<Guid, TakeVolunteerRequestToReviewCommand> handler,
        [FromServices] IAccountContract accountContract,
        CancellationToken cancellationToken = default)
    {
        var userIdResult = accountContract.GetCurrentUserId(HttpContext);
        if (userIdResult.IsFailure)
            return userIdResult.Error.ToResponse();

        var command = new TakeVolunteerRequestToReviewCommand(volunteerRequestId, userIdResult.Value);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{volunteerRequestId::guid}/send-to-revision")]
    [Permission(Permissions.VolunteerRequests.SendVolunteerRequestToRevision)]
    public async Task<ActionResult> SendVolunteerRequestToRevision(
        [FromRoute] Guid volunteerRequestId,
        [FromBody] SendVolunteerRequestToRevisionRequest request,
        [FromServices] ICommandHandler<Guid, SendVolunteerRequestToRevisionCommand> handler,
        CancellationToken cancellationToken = default)
    {
        var command = new SendVolunteerRequestToRevisionCommand(
            volunteerRequestId,
            request.RejectionComment);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}