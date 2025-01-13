using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.RefreshToken;

public record RefreshTokenCommand(Guid RefreshToken) : ICommand;