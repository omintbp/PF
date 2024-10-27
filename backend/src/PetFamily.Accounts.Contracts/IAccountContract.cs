namespace PetFamily.Accounts.Contracts;

public interface IAccountContract
{
    public Task<bool> CheckIfUserHasPermission(Guid userId, string permission, CancellationToken cancellation = default);
}