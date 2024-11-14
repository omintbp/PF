namespace PetFamily.Core.DTOs.Accounts;

public class UserRolesDto
{
    public Guid RoleId { get; init; }

    public RoleDto Role { get; init; } = default!;

    public Guid UserId { get; init; }

    public UserDto User { get; init; } = default!;
}