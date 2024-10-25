using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Presentation.Requests;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;
using PetFamily.Framework.Extensions;
using PetFamily.Species.Application.Commands.Login;
using PetFamily.Species.Application.Commands.Register;

namespace PetFamily.Accounts.Presentation;

public class AccountController : ApplicationController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] ICommandHandler<RegisterCommand> handler,
        CancellationToken cancellationToken = default!)
    {
        var command = request.ToCommand();

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<ActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] ICommandHandler<string, LoginCommand> handler,
        CancellationToken cancellationToken = default!)
    {
        var command = request.ToCommand();

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}