namespace PetFamily.Discussions.Contracts.DTOs;

public class MessageDto
{
    public Guid MessageId { get; init; }

    public bool IsEdited { get; init; }

    public DateTime CreatedAt { get; init; }

    public string Text { get; init; } = string.Empty;

    public Guid UserId { get; init; }

    public string Username { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;

    public string Patronymic { get; init; } = string.Empty;

    public string Surname { get; init; } = string.Empty;

    public string UserEmail { get; init; } = string.Empty;
}