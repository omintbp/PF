namespace PetFamily.Accounts.Infrastructure.Options;

public class AdminOptions
{
    public const string ADMIN = "ADMIN";

    public string Email { get; init; } = string.Empty;

    public string UserName { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;

    public string Surname { get; init; } = string.Empty;

    public string Patronymic { get; init; } = string.Empty;
}