using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public sealed class UpdateSocialNetworksHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<UpdateSocialNetworksCommand> _logger;

    public UpdateSocialNetworksHandler(
        IVolunteerRepository repository,
        ILogger<UpdateSocialNetworksCommand> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var volunteer = volunteerResult.Value;

        var socialNetworks = command.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Url, s.Name).Value);

        var volunteerSocialNetworks = new VolunteerSocialNetworks(socialNetworks);

        volunteer.UpdateSocialNetworks(volunteerSocialNetworks);

        var id = await _repository.Save(volunteer, cancellationToken);

        _logger.LogInformation("Social Networks for volunteer {id} updated", volunteerId);

        return id;
    }
}