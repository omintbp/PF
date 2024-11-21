namespace PetFamily.SharedKernel.IDs;

public record MessageId
{
    private MessageId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static MessageId NewMessageId() => new(Guid.NewGuid());

    public static MessageId Empty() => new(Guid.Empty);

    public static MessageId Create(Guid id) => new(id);
}