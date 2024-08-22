using PetFamily.Domain.Entities.SharedValueObjects;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pets;

public class Pet : Entity<PetId>
{
    private readonly List<PetPhoto> _photos = [];
    
    private Pet(PetId id): base(id) 
    {
        
    }
    
    private Pet(
        PetId id, 
        PetName name, 
        string species, 
        Description description, 
        string breed,
        Address address, 
        PhoneNumber phoneNumber, 
        HelpStatus helpStatus, 
        DateTimeOffset createdAt, 
        PaymentDetails paymentDetails,
        PetDetails details,
        List<PetPhoto> photos) 
        : base(id)
    {
        Name = name;
        Species = species;
        Description = description;
        Breed = breed;
        Address = address;
        PhoneNumber = phoneNumber;
        HelpStatus = helpStatus;
        CreatedAt = createdAt;
        PaymentDetails = paymentDetails;
        Details = details;
        _photos = photos;
    }

    public PetName Name { get; private set; }
    
    public string Species { get; private set; }
    
    public Description Description { get; private set; }
    
    public string Breed { get; private set; }
    
    public Address Address { get; private set; }
    
    public PhoneNumber PhoneNumber { get; private set; } 
    
    public HelpStatus HelpStatus { get; private set; }
    
    public PaymentDetails PaymentDetails { get; private set; }
    
    public DateTimeOffset CreatedAt { get; private set; }
    
    public PetDetails Details { get; private set; }

    public IReadOnlyList<PetPhoto> Photos => _photos;

    public static Pet Create(
        PetId id, 
        PetName name, 
        string species, 
        Description description, 
        string breed,
        Address address, 
        PhoneNumber phoneNumber, 
        DateTime birthday, 
        HelpStatus helpStatus, 
        DateTimeOffset createdAt, 
        PaymentDetails paymentDetails,
        PetDetails details,
        List<PetPhoto> photos
        )
    {
        var pet = new Pet(
           id, 
           name, 
           species, 
           description, 
           breed, 
           address, 
           phoneNumber, 
           helpStatus, 
           createdAt, 
           paymentDetails, 
           details, 
           photos
           );
        
        return pet;
    }
}