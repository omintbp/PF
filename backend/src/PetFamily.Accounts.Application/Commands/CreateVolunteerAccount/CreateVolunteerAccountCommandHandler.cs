using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Managers;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Application.Commands.CreateVolunteerAccount;

public class CreateVolunteerAccountCommandHandler
    : ICommandHandler<Guid, CreateVolunteerAccountCommand>
{
    private readonly ILogger<CreateVolunteerAccountCommand> _logger;
    private readonly IAccountManager _accountManager;
    private readonly UserManager<User> _userManager;
    private readonly IValidator<CreateVolunteerAccountCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVolunteerAccountCommandHandler(
        ILogger<CreateVolunteerAccountCommand> logger,
        IAccountManager accountManager,
        UserManager<User> userManager,
        IValidator<CreateVolunteerAccountCommand> validator,
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _accountManager = accountManager;
        _userManager = userManager;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerAccountCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (user == null)
            return Errors.General.NotFound(command.UserId).ToErrorList();

        var experience = Experience.Create(command.Experience).Value;

        var requisites = command.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value);

        var volunteerAccount = new VolunteerAccount(
            user,
            experience,
            requisites
        );

        await _accountManager.CreateVolunteerAccount(volunteerAccount, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Volunteer account {volunteerAccountId} created successfully",
            volunteerAccount.Id);

        return volunteerAccount.Id;
    }
}