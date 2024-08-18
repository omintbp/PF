namespace PetFamily.Domain.Entities.Volunteers;

public record VolunteerId
{
    private VolunteerId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; private set; }
    
    public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());
    
    public static VolunteerId Empty() => new(Guid.Empty);
    
    public static VolunteerId Create(Guid id) => new(id);
}