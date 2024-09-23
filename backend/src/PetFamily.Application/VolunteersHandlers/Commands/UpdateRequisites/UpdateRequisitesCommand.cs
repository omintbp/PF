using PetFamily.Application.Abstractions;
using PetFamily.Application.SharedDTOs;

namespace PetFamily.Application.VolunteersHandlers.Commands.UpdateRequisites;

public record UpdateRequisitesCommand(Guid VolunteerId, IEnumerable<RequisiteDto> Requisites) : ICommand;