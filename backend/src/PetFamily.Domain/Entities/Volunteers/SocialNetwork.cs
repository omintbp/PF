using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Volunteers;

public record SocialNetwork
{
    private SocialNetwork(string url, string name)
    {
        Url = url;
        Name = name;
    }
    
    public string Url { get; }
    
    public string Name { get; }

    public static Result<SocialNetwork, Error> Create(string url, string name)
    {
        if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            return Errors.General.ValueIsInvalid(nameof(url));
        
        if(string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid(nameof(name));
                
        var socialNetwork = new SocialNetwork(url, name);
        
        return socialNetwork;
    }
}