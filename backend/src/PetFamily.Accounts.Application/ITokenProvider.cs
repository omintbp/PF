using PetFamily.Accounts.Domain;

namespace PetFamily.Species.Application;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}