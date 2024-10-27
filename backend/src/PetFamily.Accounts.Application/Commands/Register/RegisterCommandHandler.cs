using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Extensions;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Application.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IValidator<RegisterCommand> _validator;

    public RegisterCommandHandler(
        ILogger<RegisterCommandHandler> logger,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IValidator<RegisterCommand> validator)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var participantRole = await _roleManager.FindByNameAsync(ParticipantAccount.Participant);
        if (participantRole == null)
            return Errors.General.NotFound().ToErrorList();

        var fullName = FullName.Create(
            command.FullName.FirstName,
            command.FullName.Surname,
            command.FullName.Patronymic).Value;

        var socialNetworks = command.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Url, s.Name).Value);

        var user = User.CreateParticipant(
            command.UserName,
            command.Email,
            fullName,
            FilePath.None,
            participantRole,
            socialNetworks);

        var createResult = await _userManager.CreateAsync(user, command.Password);

        if (createResult.Succeeded == false)
            return createResult.Errors.ToErrorList();

        //var participant = new ParticipantAccount(user);

        _logger.LogInformation("User {userName} created", command.UserName);

        return UnitResult.Success<ErrorList>();
    }
}