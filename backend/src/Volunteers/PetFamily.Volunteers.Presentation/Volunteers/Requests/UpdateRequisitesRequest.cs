using PetFamily.Core.DTOs.Shared;
using PetFamily.Volunteers.Application.Commands.UpdateRequisites;

namespace PetFamily.Volunteers.Presentation.Volunteers.Requests;

public record UpdateRequisitesRequest(IEnumerable<RequisiteDto> Requisites)
{
    public UpdateRequisitesCommand ToCommand(Guid id) =>
        new (id, Requisites);
}