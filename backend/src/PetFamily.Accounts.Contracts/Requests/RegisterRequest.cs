using PetFamily.Accounts.Application.Commands.Register;
using PetFamily.Core.DTOs.Shared;

namespace PetFamily.Accounts.Contracts.Requests;

public record RegisterRequest(
    FullNameDto FullName,
    string UserName,
    string Email,
    string Password,
    IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public RegisterCommand ToCommand() =>
        new RegisterCommand(
            FullName,
            UserName,
            Email,
            Password,
            SocialNetworks);
}