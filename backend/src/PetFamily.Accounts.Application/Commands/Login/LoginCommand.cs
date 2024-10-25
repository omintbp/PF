using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand;