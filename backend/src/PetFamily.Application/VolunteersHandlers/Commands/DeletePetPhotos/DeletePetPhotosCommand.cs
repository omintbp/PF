using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersHandlers.Commands.DeletePetPhotos;

public record DeletePetPhotosCommand(
    Guid VolunteerId, 
    Guid PetId, 
    IEnumerable<Guid> PhotosIds) : ICommand;
    