using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
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

        var email = EmailAddress.Create(request.Email).Value;

        var fullName = FullName.Create(request.FirstName, request.Surname, request.Patronymic).Value;

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;

        var description = Description.Create(request.Description).Value;

        var experience = Experience.Create(request.Experience).Value;

        var requisites = request.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value);

        var socialNetworks = request.SocialNetworks
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

        return id.Value;
    }
}