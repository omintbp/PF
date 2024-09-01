using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.PetManagement;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.Delete;

public class DeleteVolunteerRequestHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<DeleteVolunteerRequest> _logger;

    public DeleteVolunteerRequestHandler(
        IVolunteerRepository repository, 
        ILogger<DeleteVolunteerRequest> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        DeleteVolunteerRequest request, 
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(request.VolunteerId);

        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(volunteerId.Value);
        
        var volunteer = volunteerResult.Value;
        
        volunteer.Delete();

        var id  = await _repository.Save(volunteer, cancellationToken);
        
        _logger.LogInformation("Volunteer {volunteerId} was soft deleted", volunteerId);

        return id;
    }
}