namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeedingOptions
{
    public Dictionary<string, string[]> Roles { get; set; }
    
    public List<string> Permissions { get; set; }
}