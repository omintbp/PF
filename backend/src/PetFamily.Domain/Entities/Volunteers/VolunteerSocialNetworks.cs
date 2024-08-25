namespace PetFamily.Domain.Entities.Volunteers;

public record VolunteerSocialNetworks
{
    public IReadOnlyList<SocialNetwork> Values { get; }

    private VolunteerSocialNetworks()
    {
    }

    public VolunteerSocialNetworks(IEnumerable<SocialNetwork> socialNetwork)
    {
        Values = socialNetwork.ToList();
    }
}