using PetFamily.Application.SharedDTOs;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest( 
    string FirstName,
    string Surname,
    string Patronymic,
    string Email,
    string? Description,
    int Experience,
    string PhoneNumber,
    List<SocialNetworkDto> SocialNetworks,
    List<RequisiteDto> Requisites
    );