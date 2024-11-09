using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Commands.Login;
using PetFamily.Accounts.Application.Commands.RefreshToken;
using PetFamily.Accounts.Application.Commands.Register;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Accounts.Contracts.Response;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;
using PetFamily.Framework.Extensions;

namespace PetFamily.Accounts.Presentation;

public class AccountController : ApplicationController
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
        CancellationToken cancellationToken = default!)
    {
        var command = new LoginCommand(request.Email, request.Password);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshToken(
        [FromBody] RefreshTokenRequest request,
        [FromServices] ICommandHandler<LoginResponse, RefreshTokenCommand> handler,
        CancellationToken cancellationToken = default!)
    {
        var command = new RefreshTokenCommand(request.AccessToken, request.RefreshToken);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}