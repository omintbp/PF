using PetFamily.Core.DTOs.Shared;

namespace PetFamily.Core.DTOs.Accounts;

public class UserDto
{
    public Guid Id { get; init; }

    public string Email { get; init; } = string.Empty;

    public string UserName { get; init; } = string.Empty;

    public FullNameDto FullName { get; init; } = default!;

    public AdminAccountDto? AdminAccount { get; init; }

    public ParticipantAccountDto? ParticipantAccount { get; init; }

    public VolunteerAccountDto? VolunteerAccount { get; init; }

    public FilePathDto Photo { get; init; } = default!;

    public List<RoleDto> Roles { get; init; } = [];

    public List<SocialNetworkDto> SocialNetworks { get; init; } = [];
}