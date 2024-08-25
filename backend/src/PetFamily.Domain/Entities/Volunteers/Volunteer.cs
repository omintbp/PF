using PetFamily.Domain.Entities.SharedValueObjects;
using PetFamily.Domain.Entities.Volunteers.Pets;
using PetFamily.Domain.Enums;

namespace PetFamily.Domain.Entities.Volunteers;

public class Volunteer : Shared.Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    private Volunteer(VolunteerId id)
        : base(id)
    {
        
    }

    public Volunteer(
        VolunteerId id, 
        FullName fullName, 
        EmailAddress email, 
        Description description, 
        Experience experience, 
        PhoneNumber phoneNumber, 
        VolunteerDetails details) 
        : base(id)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
        Details = details;
    }

    public FullName FullName { get; private set; }

    public EmailAddress Email { get; private set; }
    
    public Description Description { get; private set; }
    
    public Experience Experience { get; private set; }
    
    public PhoneNumber PhoneNumber { get; private set; }
    
    public VolunteerDetails Details { get; private set; }
    
    public IReadOnlyCollection<Pet> Pets => _pets;
    
    
    public int GetPetsHomeFoundCount() => _pets.Count(p => p.HelpStatus == HelpStatus.FoundHome);
    
    public int GetPetsLookingForHomeCount() => _pets.Count(p => p.HelpStatus == HelpStatus.LookingForHome);
    
    public int GetPetsNeedsHelpCount() => _pets.Count(p => p.HelpStatus == HelpStatus.NeedsHelp);
}