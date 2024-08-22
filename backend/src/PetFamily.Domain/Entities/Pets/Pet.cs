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
        Description description, 
        Address address, 
        PhoneNumber phoneNumber, 
        HelpStatus helpStatus, 
        DateTimeOffset createdAt, 
        PaymentDetails paymentDetails,
        PetDetails details,
        List<PetPhoto> photos,
        SpeciesDetails speciesDetails) 
        : base(id)
    {
        Name = name;
        Description = description;
        Address = address;
        PhoneNumber = phoneNumber;
        HelpStatus = helpStatus;
        CreatedAt = createdAt;
        PaymentDetails = paymentDetails;
        Details = details;
        SpeciesDetails = speciesDetails;
        _photos = photos;
    }

    public PetName Name { get; private set; }
    
    public Description Description { get; private set; }
    
    public Address Address { get; private set; }
    
    public PhoneNumber PhoneNumber { get; private set; } 
    
    public HelpStatus HelpStatus { get; private set; }
    
    public PaymentDetails PaymentDetails { get; private set; }
    
    public DateTimeOffset CreatedAt { get; private set; }
    
    public PetDetails Details { get; private set; }
    
    public SpeciesDetails SpeciesDetails { get; private set; }

    public IReadOnlyList<PetPhoto> Photos => _photos;

    public static Pet Create(
        PetId id, 
        PetName name, 
        Description description, 
        Address address, 
        PhoneNumber phoneNumber, 
        DateTime birthday, 
        HelpStatus helpStatus, 
        DateTimeOffset createdAt, 
        PaymentDetails paymentDetails,
        PetDetails details,
        List<PetPhoto> photos,
        SpeciesDetails speciesDetails
        )
    {
        var pet = new Pet(
           id, 
           name, 
           description, 
           address, 
           phoneNumber, 
           helpStatus, 
           createdAt, 
           paymentDetails, 
           details, 
           photos,
           speciesDetails
           );
        
        return pet;
    }
}