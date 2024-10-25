using PetFamily.Species.Application.Commands.Register;

namespace PetFamily.Accounts.Presentation.Requests;

public record RegisterRequest(string UserName, string Email, string Password)
{
    public RegisterCommand ToCommand() => new RegisterCommand(UserName, Email, Password);
}