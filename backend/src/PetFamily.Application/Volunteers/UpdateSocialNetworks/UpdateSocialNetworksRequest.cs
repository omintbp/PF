namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public record UpdateSocialNetworksRequest(Guid VolunteerId, UpdateSocialNetworksDto Dto);