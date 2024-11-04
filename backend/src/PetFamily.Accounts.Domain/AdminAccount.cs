namespace PetFamily.Accounts.Domain;

public class AdminAccount
{
    public static readonly string Admin = nameof(Admin);

    private AdminAccount()
    {
    }

    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public AdminAccount(Guid userId, User user)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        User = user;
    }
}