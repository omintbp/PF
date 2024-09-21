using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public sealed class UpdateMainInfoHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<UpdateMainInfoCommand> _logger;

    public UpdateMainInfoHandler(
        IVolunteerRepository repository,
        ILogger<UpdateMainInfoCommand> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var volunteer = volunteerResult.Value;

        var fullName = FullName.Create(
            command.FullName.FirstName,
            command.FullName.Surname,
            command.FullName.Patronymic).Value;

        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        var experience = Experience.Create(command.Experience).Value;

        var description = Description.Create(command.Description).Value;

        volunteer.UpdateMainInfo(fullName, description, experience, phoneNumber);

        var id = await _repository.Save(volunteer, cancellationToken);

        _logger.LogInformation("Volunteer {id} updated", id);

        return id;
    }
}