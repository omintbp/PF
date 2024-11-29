using CSharpFunctionalExtensions;
using PetFamily.Discussions.Domain.Entities;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.Discussions.Domain.AggregateRoot;

public class Discussion : SharedKernel.Entity<DiscussionId>
{
    private readonly List<Message> _messages = [];

    private readonly List<Guid> _users = [];

    private Discussion()
    {
    }

    private Discussion(DiscussionId id, Guid relationId, List<Guid> users)
        : base(id)
    {
        RelationId = relationId;
        _users = users.ToList();
        IsActive = true;
    }

    public Guid RelationId { get; private set; }

    public bool IsActive { get; private set; }

    public IReadOnlyList<Guid> Users => _users;

    public IReadOnlyList<Message> Messages => _messages;

    public static Result<Discussion, Error> Create(Guid relationId, List<Guid> users)
    {
        if (users.Count != 2)
            return Errors.Discussion.InvalidNumberOfUsers();

        var id = DiscussionId.NewDiscussionId();

        return new Discussion(id, relationId, users);
    }

    public UnitResult<Error> AddMessage(Guid userId, Message message)
    {
        if (Users.Contains(userId) == false)
            return Errors.Discussion.UserNotInDiscussion(userId);

        _messages.Add(message);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> DeleteMessage(Guid userId, MessageId messageId)
    {
        if (Users.Contains(userId) == false)
            return Errors.Discussion.UserNotInDiscussion(userId);

        var message = _messages.FirstOrDefault(m => m.Id == messageId);

        if (message is null)
            return Errors.General.NotFound(messageId.Value);

        if (message.UserId != userId)
            return Errors.Discussion.NoRightsToEditMessage();

        _messages.Remove(message);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateMessage(MessageId messageId, Guid userId, Text text)
    {
        if (Users.Contains(userId) == false)
            return Errors.Discussion.UserNotInDiscussion(userId);

        var message = _messages.FirstOrDefault(m => m.Id == messageId);

        if (message is null)
            return Errors.General.NotFound(messageId.Value);

        if (message.UserId != userId)
            return Errors.General.ValueIsInvalid(nameof(message.UserId));

        message.UpdateText(text);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Close(Guid userId)
    {
        if (Users.Contains(userId) == false)
            return Errors.Discussion.UserNotInDiscussion(userId);

        IsActive = false;

        return UnitResult.Success<Error>();
    }
}