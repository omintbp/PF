using PetFamily.Application.VolunteersHandlers.Commands.DeletePetPhotos;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record DeletePetPhotosRequest(IEnumerable<Guid> PhotosIds)
{
    public DeletePetPhotosCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, PhotosIds);
}