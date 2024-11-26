using PetFamily.Core.DTOs.Shared;

namespace PetFamily.VolunteerRequests.Contracts.Requests;

public record UpdateVolunteerRequestRequest(
    int Experience,
    IEnumerable<RequisiteDto> Requisites);