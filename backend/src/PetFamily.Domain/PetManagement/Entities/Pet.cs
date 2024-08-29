using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.PetManagement.Entities;

public class Pet : Entity<PetId>
{
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

    public void AddPhoto(PetPhoto photo) => _photos.Add(photo);
}