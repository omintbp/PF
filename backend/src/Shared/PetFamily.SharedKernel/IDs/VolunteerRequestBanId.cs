namespace PetFamily.SharedKernel.IDs;

public record VolunteerRequestBanId
{
    private VolunteerRequestBanId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static VolunteerRequestBanId NewVolunteerRequestBanId() => new(Guid.NewGuid());

    public static VolunteerRequestBanId Empty() => new(Guid.Empty);

    public static VolunteerRequestBanId Create(Guid id) => new(id);
}