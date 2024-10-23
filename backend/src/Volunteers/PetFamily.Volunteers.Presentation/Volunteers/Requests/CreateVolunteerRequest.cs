using PetFamily.Core.DTOs.Shared;
using PetFamily.Volunteers.Application.Commands.Create;

namespace PetFamily.Volunteers.Presentation.Volunteers.Requests;

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