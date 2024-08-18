namespace PetFamily.Domain.Entities.ValueObjects;

public record SocialNetwork
{
    private SocialNetwork(string url, string name)
    {
        Url = url;
        Name = name;
    }
    
    public string Url { get; private set; }
    
    public string Name { get; private set; }

    public static SocialNetwork Create(string url, string name)
    {
        var socialNetwork = new SocialNetwork(url, name);
        
        return socialNetwork;
    }
}