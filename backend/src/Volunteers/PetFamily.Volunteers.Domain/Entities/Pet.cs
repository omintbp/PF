using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.AggregateRoot;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Domain.Entities;

public class Pet : SoftDeletableEntity<PetId>
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

    public UnitResult<Error> SetMainPhoto(PetPhotoId photoId)
    {
        var photoResult = _photos.FirstOrDefault(p => p.Id == photoId);

        if (photoResult is null)
            return Errors.General.NotFound(photoId.Value);

        foreach (var photo in _photos)
        {
            photo.SetAsNotMain();
        }
        
        photoResult.SetAsMain();
        
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> DeletePhoto(PetPhotoId photoId)
    {
        var photo = _photos.FirstOrDefault(p => p.Id == photoId);

        if (photo is null)
            return Errors.General.NotFound();

        _photos.Remove(photo);
        
        return UnitResult.Success<Error>();
    }

    public Result<PetPhoto, Error> GetPhotoById(PetPhotoId photoId)
    {
        var photo = _photos.FirstOrDefault(p => p.Id == photoId);

        if (photo is null)
            return Errors.General.NotFound();

        return photo;
    }
    
    public Position Position { get; private set; }
    
    public void SetPosition(Position position) => Position = position;

    public void Update(  
        PetName name,
        Description description,
        Address address,
        PhoneNumber phoneNumber,
        HelpStatus helpStatus,
        PaymentDetails paymentDetails,
        PetDetails details,
        SpeciesDetails speciesDetails)
    {
        Name = name;
        Description = description;
        Address = address;
        PhoneNumber = phoneNumber;
        HelpStatus = helpStatus;
        PaymentDetails = paymentDetails;
        Details = details;
        SpeciesDetails = speciesDetails;
    }
    
    public void UpdateStatus(HelpStatus newStatus) => HelpStatus = newStatus;
    
    public override void Delete()
    {
        base.Delete();

        foreach (var photo in _photos)
        {
            photo.Delete();
        }
    }

    public override void Restore()
    {
        base.Restore();
        
        foreach (var photo in _photos)
        {
            photo.Restore();
        }
    }
}