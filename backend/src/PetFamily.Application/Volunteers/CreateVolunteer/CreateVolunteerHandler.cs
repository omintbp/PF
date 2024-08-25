using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.SharedValueObjects;
using PetFamily.Domain.Entities.Volunteers;
using PetFamily.Domain.Shared;

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
        
        if(phoneNumber.IsFailure)
            return phoneNumber.Error;
        
        var description = Description.Create(request.Description);
        
        if(description.IsFailure)
            return description.Error;
        
        var experience = Experience.Create(request.Experience);
        
        if(experience.IsFailure)
            return experience.Error;

        var requisites = request.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description))
            .ToList();

        var requisitesErrors = requisites.Where(r => r.IsFailure)
            .Select(r => r.Error.Message)
            .ToList();

        if (requisitesErrors.Count != 0)
        {
            var errorMessage = string.Join(';', requisitesErrors);
            
            return Error.Validation("values.is.invalid", errorMessage);
        }
        
        var socialNetworks = request.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Url, s.Name))
            .ToList();
        
        var socialNetworksErrors = socialNetworks.Where(r => r.IsFailure)
            .Select(r => r.Error.Message)
            .ToList();

        if (socialNetworksErrors.Count != 0)
        {
            var errorMessage = string.Join(';', socialNetworksErrors);
            
            return Error.Validation("values.is.invalid", errorMessage);
        }

        var details = VolunteerDetails.Create(
            requisites.Select(r => r.Value).ToList(), 
            socialNetworks.Select(s => s.Value).ToList());
        
        var volunteer = new Volunteer(
            volunteerId, 
            fullName.Value, 
            email.Value, 
            description.Value, 
            experience.Value, 
            phoneNumber.Value, 
            details
            );

        var id = await _repository.Add(volunteer, cancellationToken);
        
        return id.Value;
    }
}