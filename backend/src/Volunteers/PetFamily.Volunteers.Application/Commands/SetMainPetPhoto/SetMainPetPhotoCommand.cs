using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Commands.SetMainPetPhoto;

public record SetMainPetPhotoCommand(Guid VolunteerId, Guid PetId, Guid PhotoId) : ICommand;