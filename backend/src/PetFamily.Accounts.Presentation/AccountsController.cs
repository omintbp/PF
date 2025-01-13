using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Commands.Login;
using PetFamily.Accounts.Application.Commands.RefreshToken;
using PetFamily.Accounts.Application.Commands.Register;
using PetFamily.Accounts.Application.Managers;
using PetFamily.Accounts.Application.Queries.GetUserById;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Accounts.Contracts.Response;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs.Accounts;
using PetFamily.Framework;
using PetFamily.Framework.Extensions;

namespace PetFamily.Accounts.Presentation;

public class AccountsController : ApplicationController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] ICommandHandler<RegisterCommand> handler,
        CancellationToken cancellationToken = default!)
    {
        var command = new RegisterCommand(
            request.FullName,
            request.UserName,
            request.Email,
            request.Password,
            request.SocialNetworks);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] ICommandHandler<LoginResponse, LoginCommand> handler,
        [FromServices] IRefreshSessionManager refreshSessionManager,
        CancellationToken cancellationToken = default!)
    {
        var command = new LoginCommand(request.Email, request.Password);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var setRefreshSessionCookieResult = refreshSessionManager.SetRefreshSessionCookie(result.Value.RefreshToken);
        if (setRefreshSessionCookieResult.IsFailure)
            return setRefreshSessionCookieResult.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshToken(
        [FromServices] ICommandHandler<LoginResponse, RefreshTokenCommand> handler,
        [FromServices] IRefreshSessionManager refreshSessionManager,
        CancellationToken cancellationToken = default!)
    {
        var getRefreshSessionCookieResult = refreshSessionManager.GetRefreshSessionCookie();
        if (getRefreshSessionCookieResult.IsFailure)
            return Unauthorized();

        var command = new RefreshTokenCommand(getRefreshSessionCookieResult.Value);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        var setRefreshSessionCookieRes = refreshSessionManager.SetRefreshSessionCookie(result.Value.RefreshToken);
        if (setRefreshSessionCookieRes.IsFailure)
            return setRefreshSessionCookieRes.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet("{userId::guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid userId,
        [FromServices] IQueryHandler<UserDto, GetUserByIdQuery> handler,
        CancellationToken cancellationToken = default!)
    {
        var query = new GetUserByIdQuery(userId);

        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}