using PetFamily.Application.SharedDTOs;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public record UpdateRequisitesCommand(Guid VolunteerId, IEnumerable<RequisiteDto> Requisites);