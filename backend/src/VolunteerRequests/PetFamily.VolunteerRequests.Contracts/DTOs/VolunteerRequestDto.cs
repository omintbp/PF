using PetFamily.Core.DTOs.Shared;

namespace PetFamily.VolunteerRequests.Contracts.DTOs;

public class VolunteerRequestDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid AdminId { get; set; }

    public Guid DiscussionId { get; set; }

    public int Experience { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<RequisiteDto> Requisites { get; set; } = [];

    public string Status { get; set; } = string.Empty;

    public string RejectionComment { get; set; } = String.Empty;
}