using PetFamily.Domain.Enums;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pets;

public class Pet : Entity<PetId>
{
    private Pet() 
    {
        
    }
    
    private Pet(PetId id, string name, string species, string description, string breedName,
        string color, string healthInfo, Address address, double weight, double height,
        string phoneNumber, bool isCastrated, DateTime birthday, bool isVaccinated,
        HelpStatus helpStatus, DateTimeOffset createdAt, PaymentDetails paymentDetails) 
        : base(id)
    {
        Name = name;
        Species = species;
        Description = description;
        BreedName = breedName;
        Color = color;
        HealthInfo = healthInfo;
        Address = address;
        Weight = weight;
        Height = height;
        PhoneNumber = phoneNumber;
        IsCastrated = isCastrated;
        Birthday = birthday;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        CreatedAt = createdAt;
        PaymentDetails = paymentDetails;
    }

    public string Name { get; private set; }
    
    public string Species { get; private set; }
    
    public string Description { get; private set; }
    
    public string BreedName { get; private set; }
    
    public string Color { get; private set; }
    
    public string HealthInfo { get; private set; }
    
    public Address Address { get; private set; }
    
    public double Weight { get; private set; }

    public double Height { get; private set; }
    
    public string PhoneNumber { get; private set; } 
    
    public bool IsCastrated { get; private set; }
    
    public DateTime Birthday { get; private set; }
    
    public bool IsVaccinated { get; private set; }
    
    public HelpStatus HelpStatus { get; private set; }
    
    public PaymentDetails PaymentDetails { get; private set; }
    
    public DateTimeOffset CreatedAt { get; private set; }

    public static Pet Create(PetId id, string name, string species, string description, string breedName,
        string color, string healthInfo, Address address, double weight, double height,
        string phoneNumber, bool isCastrated, DateTime birthday, bool isVaccinated,
        HelpStatus helpStatus, DateTimeOffset createdAt, PaymentDetails paymentDetails)
    {
        var pet = new Pet(id, name, species, description, breedName,color,
            healthInfo, address, weight, height, phoneNumber, isCastrated, birthday,
            isVaccinated, helpStatus, createdAt, paymentDetails);
        
        return pet;
    }
}