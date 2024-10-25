using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Species.Application.Extensions;

namespace PetFamily.Species.Application.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IValidator<RegisterCommand> _validator;

    public RegisterCommandHandler(
        ILogger<RegisterCommandHandler> logger,
        UserManager<User> userManager,
        IValidator<RegisterCommand> validator)
    {
        _logger = logger;
        _userManager = userManager;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        RegisterCommand command, 
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();
        
        var user = new User()
        {
            Email = command.Email,
            UserName = command.UserName
        };

        var createResult = await _userManager.CreateAsync(user, command.Password);

        if (createResult.Succeeded == false)
            return createResult.Errors.ToErrorList();

        _logger.LogInformation("User {userName} created", command.UserName);

        return UnitResult.Success<ErrorList>();
    }
}