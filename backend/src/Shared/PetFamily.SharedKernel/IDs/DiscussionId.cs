namespace PetFamily.SharedKernel.IDs;

public record DiscussionId
{
    private DiscussionId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static DiscussionId NewDiscussionId() => new(Guid.NewGuid());

    public static DiscussionId Empty() => new(Guid.Empty);

    public static DiscussionId Create(Guid id) => new(id);
}