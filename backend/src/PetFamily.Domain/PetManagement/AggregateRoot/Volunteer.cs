using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.PetManagement.AggregateRoot;

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
        VolunteerRequisites requisites,
        VolunteerSocialNetworks socialNetworks)
        : base(id)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
        Requisites = requisites;
        SocialNetworks = socialNetworks;
    }

    public FullName FullName { get; private set; }

    public EmailAddress Email { get; private set; }

    public Description Description { get; private set; }

    public Experience Experience { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public VolunteerRequisites Requisites { get; private set; }

    public VolunteerSocialNetworks SocialNetworks { get; private set; }

    public IReadOnlyCollection<Pet> Pets => _pets;

    public int GetPetsHomeFoundCount() =>
        _pets.Count(p => p.HelpStatus == HelpStatus.FoundHome);

    public int GetPetsLookingForHomeCount() =>
        _pets.Count(p => p.HelpStatus == HelpStatus.LookingForHome);

    public int GetPetsNeedsHelpCount() =>
        _pets.Count(p => p.HelpStatus == HelpStatus.NeedsHelp);

    public void AddPet(Pet pet) =>
        _pets.Add(pet);

    public void UpdateRequisites(VolunteerRequisites requisites) =>
        Requisites = requisites;

    public void UpdateSocialNetworks(VolunteerSocialNetworks socialNetworks) =>
        SocialNetworks = socialNetworks;
}