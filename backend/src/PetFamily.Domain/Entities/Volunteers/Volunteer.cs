using PetFamily.Domain.Entities.Pets;
using PetFamily.Domain.Entities.ValueObjects;
using PetFamily.Domain.Enums;

namespace PetFamily.Domain.Entities.Volunteers;

public class Volunteer : Shared.Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    private readonly List<SocialNetwork> _socialNetworks = [];

    private Volunteer(VolunteerId id, FullName fullName, EmailAddress email, Description description, 
        Experience experience, PhoneNumber phoneNumber, PaymentDetails paymentDetails, List<Pet> pets,
        List<SocialNetwork> socialNetworks) : base(id)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
        PaymentDetails = paymentDetails;
        _pets = pets;
        _socialNetworks = socialNetworks;
    }

    public FullName FullName { get; private set; }

    public EmailAddress Email { get; private set; }
    
    public Description Description { get; private set; }
    
    public Experience Experience { get; private set; }
    
    public PhoneNumber PhoneNumber { get; private set; }
    
    public PaymentDetails PaymentDetails { get; private set; }
    
    public IReadOnlyCollection<Pet> Pets => _pets;
    
    public IReadOnlyCollection<SocialNetwork> SocialNetworks => _socialNetworks;
    
    public int GetPetsHomeFoundCount() => _pets.Count(p => p.HelpStatus == HelpStatus.FoundHome);
    
    public int GetPetsLookingForHomeCount() => _pets.Count(p => p.HelpStatus == HelpStatus.LookingForHome);
    
    public int GetPetsNeedsHelpCount() => _pets.Count(p => p.HelpStatus == HelpStatus.NeedsHelp);

    public static Volunteer Create(VolunteerId id, FullName fullName, EmailAddress email, Description description, 
        Experience experience, PhoneNumber phoneNumber, PaymentDetails paymentDetails, List<Pet> pets,
        List<SocialNetwork> socialNetworks)
    {
        var volunteer = new Volunteer(id, fullName, email, description, experience, phoneNumber, paymentDetails, pets, socialNetworks);

        return volunteer;
    } 
}