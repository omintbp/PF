using PetFamily.Core.DTOs.Shared;

namespace PetFamily.Core.DTOs.Accounts;

public class VolunteerAccountDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public int Experience { get; init; }

    public IEnumerable<RequisiteDto> Requisites { get; init; } = [];
}