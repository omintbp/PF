using PetFamily.Core.DTOs.Shared;

namespace PetFamily.VolunteerRequests.Contracts.Requests;

public record CreateVolunteerRequestRequest(
    int Experience,
    IEnumerable<RequisiteDto> Requisites);