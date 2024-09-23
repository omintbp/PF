using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Application.VolunteersHandlers.Commands.Delete;

public class DeleteVolunteerRequestHandler : ICommandHandler<Guid, DeleteVolunteerCommand>
{
    private readonly IVolunteerRepository _repository;
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly ILogger<DeleteVolunteerCommand> _logger;

    public DeleteVolunteerRequestHandler(
        IVolunteerRepository repository, 
        IValidator<DeleteVolunteerCommand> validator,
        ILogger<DeleteVolunteerCommand> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteVolunteerCommand command, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(volunteerId.Value).ToErrorList();
        
        var volunteer = volunteerResult.Value;
        
        volunteer.Delete();

        var id  = await _repository.Save(volunteer, cancellationToken);
        
        _logger.LogInformation("Volunteer {volunteerId} was soft deleted", volunteerId);

        return id;
    }
}