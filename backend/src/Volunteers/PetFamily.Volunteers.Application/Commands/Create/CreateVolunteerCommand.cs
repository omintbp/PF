using PetFamily.Core.DTOs.Shared;
using ICommand = PetFamily.Core.Abstractions.ICommand;

namespace PetFamily.Volunteers.Application.Commands.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Email,
    string? Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<RequisiteDto> Requisites
) : ICommand;