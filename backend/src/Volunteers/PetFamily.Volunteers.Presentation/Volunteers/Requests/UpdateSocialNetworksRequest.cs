using PetFamily.Core.DTOs.Shared;
using PetFamily.Volunteers.Application.Commands.UpdateSocialNetworks;

namespace PetFamily.Volunteers.Presentation.Volunteers.Requests;

public record UpdateSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateSocialNetworksCommand ToCommand(Guid id) =>
        new(id, SocialNetworks);
}