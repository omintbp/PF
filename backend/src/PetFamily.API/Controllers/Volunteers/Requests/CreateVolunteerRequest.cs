using PetFamily.Application.SharedDTOs;
using PetFamily.Application.Volunteers.Commands.Create;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Email,
    string? Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<RequisiteDto> Requisites
)
{
    public CreateVolunteerCommand ToCommand() =>
        new(
            FullName,
            Email,
            Description,
            Experience,
            PhoneNumber,
            SocialNetworks,
            Requisites);
}