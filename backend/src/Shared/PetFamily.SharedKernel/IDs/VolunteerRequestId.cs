namespace PetFamily.SharedKernel.IDs;

public record VolunteerRequestId
{
    private VolunteerRequestId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static VolunteerRequestId NewVolunteerRequestId() => new(Guid.NewGuid());

    public static VolunteerRequestId Empty() => new(Guid.Empty);

    public static VolunteerRequestId Create(Guid id) => new(id);
}