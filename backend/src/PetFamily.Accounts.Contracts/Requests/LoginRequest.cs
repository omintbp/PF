using PetFamily.Accounts.Application.Commands.Login;

namespace PetFamily.Accounts.Contracts.Requests;

public record LoginRequest(string Email, string Password)
{
    public LoginCommand ToCommand() => new LoginCommand(Email, Password);
}