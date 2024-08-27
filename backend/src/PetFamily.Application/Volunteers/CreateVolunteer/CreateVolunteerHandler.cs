using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public sealed class CreateVolunteerHandler
{
    private readonly IVolunteerRepository _repository;

    public CreateVolunteerHandler(IVolunteerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();

        var email = EmailAddress.Create(request.Email);

        if (email.IsFailure)
            return email.Error;

        var fullName = FullName.Create(request.FirstName, request.Surname, request.Patronymic);

        if (fullName.IsFailure)
            return fullName.Error;

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);

        if (phoneNumber.IsFailure)
            return phoneNumber.Error;

        var description = Description.Create(request.Description);

        if (description.IsFailure)
            return description.Error;

        var experience = Experience.Create(request.Experience);

        if (experience.IsFailure)
            return experience.Error;

        var requisites = request.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description))
            .ToList();

        if (requisites.Any(r => r.IsFailure))
        {
            var requisitesErrors = requisites.Where(r => r.IsFailure)
                .Select(r => r.Error.Message);

            var errorMessage = string.Join(';', requisitesErrors);

            return Error.Validation("values.is.invalid", errorMessage);
        }

        var socialNetworks = request.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Url, s.Name))
            .ToList();

        if (socialNetworks.Any(s => s.IsFailure))
        {
            var socialNetworksErrors = socialNetworks.Where(r => r.IsFailure)
                .Select(r => r.Error.Message);

            var errorMessage = string.Join(';', socialNetworksErrors);

            return Error.Validation("values.is.invalid", errorMessage);
        }

        var requisitesResult = new VolunteerRequisites(requisites.Select(r => r.Value));
        var socialNetworksResult = new VolunteerSocialNetworks(socialNetworks.Select(s => s.Value));

        var volunteer = new Volunteer(
            volunteerId,
            fullName.Value,
            email.Value,
            description.Value,
            experience.Value,
            phoneNumber.Value,
            requisitesResult,
            socialNetworksResult
        );

        var id = await _repository.Add(volunteer, cancellationToken);

        return id.Value;
    }
}