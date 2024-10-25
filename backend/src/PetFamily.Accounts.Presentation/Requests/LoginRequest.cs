using PetFamily.Species.Application.Commands.Login;

namespace PetFamily.Accounts.Presentation.Requests;

public record LoginRequest(string Email, string Password)
{
    public LoginCommand ToCommand() => new LoginCommand(Email, Password);
}