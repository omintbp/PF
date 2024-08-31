using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public sealed class UpdateRequisitesHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<UpdateRequisitesRequest> _logger;

    public UpdateRequisitesHandler(
        IVolunteerRepository repository,
        ILogger<UpdateRequisitesRequest> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateRequisitesRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(request.VolunteerId);

        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var volunteer = volunteerResult.Value;

        var requisites = request.Dto.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value);

        var volunteerRequisites = new VolunteerRequisites(requisites);

        volunteer.UpdateRequisites(volunteerRequisites);

        var id = await _repository.Save(volunteer, cancellationToken);

        _logger.LogInformation("Requisites for volunteer {id} updated", id);

        return id;
    }
}