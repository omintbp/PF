using PetFamily.Application.Abstractions;
using PetFamily.Application.SharedDTOs;

namespace PetFamily.Application.VolunteersHandlers.Commands.UpdateSocialNetworks;

public record UpdateSocialNetworksCommand(Guid VolunteerId, IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;