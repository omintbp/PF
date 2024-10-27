namespace PetFamily.Accounts.Domain;

public class ParticipantAccount
{
    public static readonly string Participant = nameof(Participant);

    private ParticipantAccount()
    {
        
    }
    
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public ParticipantAccount(User user)
    {
        User = user;
        UserId = user.Id;
        Id = Guid.NewGuid();
    }
}