using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Extensions;
using PetFamily.Accounts.Application.Managers;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Application.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountManager _accountManager;
    private readonly IValidator<RegisterCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
        ILogger<RegisterCommandHandler> logger,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IAccountManager accountManager,
        IValidator<RegisterCommand> validator,
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _accountManager = accountManager;
        _validator = validator;
        _unitOfWork = unitOfWork;
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

        var userResult = User.CreateParticipant(
            command.UserName,
            command.Email,
            fullName,
            FilePath.None,
            participantRole,
            socialNetworks);

        if (userResult.IsFailure)
            return userResult.Error.ToErrorList();

        var user = userResult.Value;

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var createResult = await _userManager.CreateAsync(user, command.Password);

            if (createResult.Succeeded == false)
                return createResult.Errors.ToErrorList();

            var participant = new ParticipantAccount(user);

            await _accountManager.CreateParticipantAccount(participant, cancellationToken);

            await _unitOfWork.SaveChanges(cancellationToken);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
        }

        transaction.Commit();

        _logger.LogInformation("User {userName} created", command.UserName);

        return UnitResult.Success<ErrorList>();
    }
}