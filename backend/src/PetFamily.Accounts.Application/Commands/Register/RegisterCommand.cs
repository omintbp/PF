using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs.Shared;

namespace PetFamily.Accounts.Application.Commands.Register;

public record RegisterCommand(
    FullNameDto FullName,
    string UserName,
    string Email,
    string Password,
    IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;