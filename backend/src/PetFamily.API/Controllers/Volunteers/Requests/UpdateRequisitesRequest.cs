using PetFamily.Application.SharedDTOs;
using PetFamily.Application.VolunteersHandlers.Commands.UpdateRequisites;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateRequisitesRequest(IEnumerable<RequisiteDto> Requisites)
{
    public UpdateRequisitesCommand ToCommand(Guid id) =>
        new (id, Requisites);
}