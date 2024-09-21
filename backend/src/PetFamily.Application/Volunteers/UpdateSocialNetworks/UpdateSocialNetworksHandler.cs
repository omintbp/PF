using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public sealed class UpdateSocialNetworksHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly IValidator<UpdateSocialNetworksCommand> _validator;
    private readonly ILogger<UpdateSocialNetworksCommand> _logger;

    public UpdateSocialNetworksHandler(
        IVolunteerRepository repository,
        IValidator<UpdateSocialNetworksCommand> validator,
        ILogger<UpdateSocialNetworksCommand> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

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