using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs.Shared;

namespace PetFamily.Application.VolunteersHandlers.Commands.UpdateSocialNetworks;

public record UpdateSocialNetworksCommand(Guid VolunteerId, IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;