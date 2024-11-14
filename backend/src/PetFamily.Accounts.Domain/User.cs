using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    private List<Role> _roles = [];

    private User()
    {
    }

    public FullName FullName { get; set; }

    public FilePath Photo { get; set; }

    public IReadOnlyList<SocialNetwork> SocialsNetworks { get; set; }

    public IReadOnlyList<Role> Roles => _roles;

    public AdminAccount? AdminAccount { get; set; }

    public VolunteerAccount? VolunteerAccount { get; set; }

    public ParticipantAccount? ParticipantAccount { get; set; }

    public static Result<User, Error> CreateParticipant(
        string userName,
        string email,
        FullName fullName,
        FilePath photo,
        Role role,
        IEnumerable<SocialNetwork> socialNetworks)
    {
        if (role.Name != ParticipantAccount.Participant)
            return Errors.General.ValueIsInvalid(nameof(role));

        return new User()
        {
            UserName = userName,
            Email = email,
            FullName = fullName,
            Photo = photo,
            _roles = [role],
            SocialsNetworks = socialNetworks.ToList()
        };
    }

    public static Result<User, Error> CreateAdmin(
        string userName,
        string email,
        FullName fullName,
        FilePath photo,
        Role role,
        IEnumerable<SocialNetwork> socialNetworks)
    {
        if (role.Name != AdminAccount.Admin)
            return Errors.General.ValueIsInvalid(nameof(role));

        return new User()
        {
            UserName = userName,
            Email = email,
            Photo = photo,
            FullName = fullName,
            _roles = [role],
            SocialsNetworks = socialNetworks.ToList()
        };
    }
}