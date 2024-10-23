using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.AggregateRoot;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Create;

public sealed class CreateVolunteerHandler : ICommandHandler<Guid, CreateVolunteerCommand>
{
    private readonly IVolunteerRepository _repository;
    private readonly IValidator<CreateVolunteerCommand> _validator;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteerRepository repository,
        IValidator<CreateVolunteerCommand> validator,
        ILogger<CreateVolunteerHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var volunteerId = VolunteerId.NewVolunteerId();

        var email = EmailAddress.Create(command.Email).Value;

        var fullName = FullName.Create(
                command.FullName.FirstName,
                command.FullName.Surname,
                command.FullName.Patronymic)
            .Value;

        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        var description = Description.Create(command.Description).Value;

        var experience = Experience.Create(command.Experience).Value;

        var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value);

        var socialNetworks = command.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Url, s.Name).Value);

        var volunteerRequisites = new VolunteerRequisites(requisites);
        var volunteerSocialNetworks = new VolunteerSocialNetworks(socialNetworks);

        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            email,
            description,
            experience,
            phoneNumber,
            volunteerRequisites,
            volunteerSocialNetworks
        );

        var id = await _repository.Add(volunteer, cancellationToken);

        _logger.LogInformation("Created volunteer with ID: {id}", id);

        return id.Value;
    }
}