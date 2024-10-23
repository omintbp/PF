using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Commands.UpdatePetStatus;

namespace PetFamily.Volunteers.Presentation.Volunteers.Requests;

public record UpdatePetStatusRequest(HelpStatus Status)
{
    public UpdatePetStatusCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, Status);
}