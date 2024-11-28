using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain;

public class VolunteerAccount
{
    public static readonly string Volunteer = nameof(Volunteer);

    private readonly List<Requisite> _requisites;

    private VolunteerAccount()
    {
        
    }
    
    public VolunteerAccount(
        User user,
        Experience experience,
        IEnumerable<Requisite> requisites)
    {
        User = user;
        Experience = experience;
        _requisites = requisites.ToList();
    }

    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public Experience Experience { get; set; }

    public IReadOnlyList<Requisite> Requisites => _requisites;
}