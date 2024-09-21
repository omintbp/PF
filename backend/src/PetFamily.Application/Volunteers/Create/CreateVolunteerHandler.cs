using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.Create;

public sealed class CreateVolunteerHandler
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