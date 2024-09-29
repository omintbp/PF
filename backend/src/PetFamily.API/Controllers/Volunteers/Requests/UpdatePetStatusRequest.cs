using PetFamily.Application.VolunteersHandlers.Commands.UpdatePetStatus;
using PetFamily.Domain.PetManagement;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdatePetStatusRequest(HelpStatus Status)
{
    public UpdatePetStatusCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, Status);
}