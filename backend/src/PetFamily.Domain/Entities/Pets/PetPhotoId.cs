namespace PetFamily.Domain.Entities.Pets;

public record PetPhotoId
{ 
    private PetPhotoId(Guid value) 
    { 
        Value = value; 
    } 
    
    public Guid Value { get; }
    
    public static PetPhotoId NewPetPhotoId() => new(Guid.NewGuid());
    
    public static PetPhotoId Empty() => new(Guid.Empty);
    
    public static PetPhotoId Create(Guid id) => new(id);
}