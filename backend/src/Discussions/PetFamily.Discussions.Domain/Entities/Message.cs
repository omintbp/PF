using CSharpFunctionalExtensions;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.Discussions.Domain.Entities;

public class Message : SharedKernel.Entity<MessageId>
{
    private Message()
    {
    }

    private Message(MessageId messageId, Guid userId, Text text)
        : base(messageId)
    {
        Text = text;
        CreatedAt = DateTime.UtcNow;
        IsEdited = false;
        UserId = userId;
    }

    public Text Text { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public bool IsEdited { get; private set; }

    public Guid UserId { get; private set; }

    public static Result<Message, Error> Create(Guid userId, Text text)
    {
        if (userId == Guid.Empty)
            return Errors.General.ValueIsInvalid(nameof(userId));

        var messageId = MessageId.NewMessageId();

        return new Message(messageId, userId, text);
    }

    internal void UpdateText(Text updatedText)
    {
        Text = updatedText;
        IsEdited = true;
    }
}