using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs.Shared;

namespace PetFamily.Application.VolunteersHandlers.Commands.UpdateRequisites;

public record UpdateRequisitesCommand(Guid VolunteerId, IEnumerable<RequisiteDto> Requisites) : ICommand;