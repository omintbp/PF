using PetFamily.Core.DTOs.Shared;

namespace PetFamily.VolunteerRequests.Contracts.DTOs;

public record VolunteerInfoDto(int Experience, IEnumerable<RequisiteDto> Requisites);