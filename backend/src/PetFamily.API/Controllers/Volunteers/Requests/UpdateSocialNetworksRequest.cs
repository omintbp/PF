using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.VolunteersHandlers.Commands.UpdateSocialNetworks;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateSocialNetworksCommand ToCommand(Guid id) =>
        new(id, SocialNetworks);
}