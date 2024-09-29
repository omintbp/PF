using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.PetManagement.Entities;

public class Pet : Entity<PetId>, ISoftDeletable
{
    private bool _isDeleted = false;
    
    private readonly List<PetPhoto> _photos = [];

    private Pet(PetId id) : base(id)
    {
    }

    public Pet(
        PetId id,
        PetName name,
        Description description,
        Address address,
        PhoneNumber phoneNumber,
        HelpStatus helpStatus,
        DateTime createdAt,
        PaymentDetails paymentDetails,
        PetDetails details,
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
    }

    public PetName Name { get; private set; }

    public Description Description { get; private set; }

    public Address Address { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public HelpStatus HelpStatus { get; private set; }

    public PaymentDetails PaymentDetails { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public PetDetails Details { get; private set; }

    public SpeciesDetails SpeciesDetails { get; private set; }

    public IReadOnlyList<PetPhoto> Photos => _photos;

    public void AddPhoto(PetPhoto photo) => _photos.Add(photo);
    
    public Position Position { get; private set; }
    
    public void SetPosition(Position position) => Position = position;
    
    public void Delete()
    {
        _isDeleted = true;

        foreach (var photo in _photos)
        {
            photo.Delete();
        }
    }

    public void Restore()
    {
        _isDeleted = false;

        foreach (var photo in _photos)
        {
            photo.Restore();
        }
    }
}