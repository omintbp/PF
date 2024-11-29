using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Commands.AddMessage;
using PetFamily.Discussions.Application.Commands.CloseDiscussion;
using PetFamily.Discussions.Application.Commands.CreateDiscussion;
using PetFamily.Discussions.Application.Commands.DeleteMessage;
using PetFamily.Discussions.Application.Commands.UpdateMessage;
using PetFamily.Discussions.Application.Queries.GetDiscussionByRelationId;
using PetFamily.Discussions.Contracts.DTOs;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Framework.Extensions;

namespace PetFamily.Discussions.Presentation;

public class DiscussionsController : ApplicationController
{
    [HttpPost]
    [Permission(Permissions.Discussion.CreateDiscussion)]
    public async Task<ActionResult> Create(
        [FromBody] CreateDiscussionRequest request,
        [FromServices] ICommandHandler<Guid, CreateDiscussionCommand> handler,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateDiscussionCommand(request.RelationId, request.Users);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{discussionId::guid}/close")]
    [Permission(Permissions.Discussion.CloseDiscussion)]
    public async Task<ActionResult> Close(
        [FromRoute] Guid discussionId,
        [FromServices] ICommandHandler<Guid, CloseDiscussionCommand> handler,
        IAccountContract accountContract,
        CancellationToken cancellationToken = default)
    {
        var userIdResult = accountContract.GetCurrentUserId(HttpContext);
        if (userIdResult.IsFailure)
            return userIdResult.Error.ToResponse();

        var command = new CloseDiscussionCommand(userIdResult.Value, discussionId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{discussionId::guid}/messages")]
    [Permission(Permissions.Discussion.AddMessage)]
    public async Task<ActionResult> AddMessage(
        [FromRoute] Guid discussionId,
        [FromBody] AddMessageRequest request,
        [FromServices] ICommandHandler<Guid, AddMessageCommand> handler,
        IAccountContract accountContract,
        CancellationToken cancellationToken = default)
    {
        var userIdResult = accountContract.GetCurrentUserId(HttpContext);
        if (userIdResult.IsFailure)
            return userIdResult.Error.ToResponse();

        var command = new AddMessageCommand(
            discussionId,
            userIdResult.Value,
            request.Message);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{discussionId::guid}/messages/{messageId::guid}")]
    [Permission(Permissions.Discussion.UpdateMessage)]
    public async Task<ActionResult> UpdateMessage(
        [FromRoute] Guid discussionId,
        [FromRoute] Guid messageId,
        [FromBody] UpdateMessageRequest request,
        [FromServices] ICommandHandler<Guid, UpdateMessageCommand> handler,
        IAccountContract accountContract,
        CancellationToken cancellationToken = default)
    {
        var userIdResult = accountContract.GetCurrentUserId(HttpContext);
        if (userIdResult.IsFailure)
            return userIdResult.Error.ToResponse();

        var command = new UpdateMessageCommand(
            discussionId,
            messageId,
            userIdResult.Value,
            request.Message);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpDelete("{discussionId::guid}/messages/{messageId::guid}")]
    [Permission(Permissions.Discussion.DeleteMessage)]
    public async Task<ActionResult> DeleteMessage(
        [FromRoute] Guid discussionId,
        [FromRoute] Guid messageId,
        [FromServices] ICommandHandler<Guid, DeleteMessageCommand> handler,
        IAccountContract accountContract,
        CancellationToken cancellationToken = default)
    {
        var userIdResult = accountContract.GetCurrentUserId(HttpContext);
        if (userIdResult.IsFailure)
            return userIdResult.Error.ToResponse();

        var command = new DeleteMessageCommand(
            discussionId,
            messageId,
            userIdResult.Value);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpGet("{relationId::guid}")]
    [Permission(Permissions.Discussion.GetByRelationId)]
    public async Task<ActionResult> GetByRelationId(
        [FromRoute] Guid relationId,
        [FromQuery] GetDisussionByRelationIdRequest request,
        [FromServices] IQueryHandler<DiscussionDto, GetDiscussionByRelationIdQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetDiscussionByRelationIdQuery(
            relationId,
            request.Page,
            request.PageSize);

        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}