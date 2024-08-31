using PetFamily.Application.SharedDTOs;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public record UpdateSocialNetworksDto(IEnumerable<SocialNetworkDto> SocialNetworks);