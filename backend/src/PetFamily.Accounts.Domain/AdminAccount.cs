namespace PetFamily.Accounts.Domain;

public class AdminAccount
{
    public static readonly string Admin = nameof(Admin);
    
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public User User { get; set; }
}