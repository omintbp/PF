using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs.Shared;

namespace PetFamily.Application.VolunteersHandlers.Commands.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Email,
    string? Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<RequisiteDto> Requisites
) : ICommand;