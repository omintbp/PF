using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersHandlers.Commands.SetMainPetPhoto;

public record SetMainPetPhotoCommand(Guid VolunteerId, Guid PetId, Guid PhotoId) : ICommand;