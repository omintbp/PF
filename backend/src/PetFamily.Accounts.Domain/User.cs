using Microsoft.AspNetCore.Identity;
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

    public static User CreateParticipant(
        string userName,
        string email,
        FullName fullName,
        FilePath photo,
        Role role,
        IEnumerable<SocialNetwork> socialNetworks)
    {
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
}