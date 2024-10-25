using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Commands.Login;

public class LoginCommandHandler : ICommandHandler<string, LoginCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IValidator<LoginCommand> _validator;

    public LoginCommandHandler(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
        ILogger<LoginCommandHandler> logger,
        IValidator<LoginCommand> validator)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<string, ErrorList>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
            return Errors.General.NotFound().ToErrorList();

        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        if (!passwordConfirmed)
            return Errors.User.InvalidCredentials().ToErrorList();

        var token = _tokenProvider.GenerateAccessToken(user);

        _logger.LogInformation("Successfully logged in.");

        return token;
    }
}