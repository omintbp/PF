using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public record SocialNetwork
{
    public static int MAX_URL_LENGTH = 8000;

    private SocialNetwork(string url, string name)
    {
        Url = url;
        Name = name;
    }

    public string Url { get; }

    public string Name { get; }

    public static Result<SocialNetwork, Error> Create(string url, string name)
    {
        if (string.IsNullOrWhiteSpace(url)
            || !Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute)
            || url.Length > MAX_URL_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(url));

        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(name));

        var socialNetwork = new SocialNetwork(url, name);

        return socialNetwork;
    }
}