using PetFamily.Application.SharedDTOs;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public record UpdateRequisitesDto(IEnumerable<RequisiteDto> Requisites);