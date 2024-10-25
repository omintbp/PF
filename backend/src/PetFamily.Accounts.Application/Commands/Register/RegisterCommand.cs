using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.Register;

public record RegisterCommand(string UserName, string Email, string Password) : ICommand;