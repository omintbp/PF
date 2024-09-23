using PetFamily.Application.Abstractions;
using PetFamily.Application.SharedDTOs;

namespace PetFamily.Application.Volunteers.Commands.AddPetPhotos;

public record AddPetPhotosCommand(Guid VolunteerId, Guid PetId, IEnumerable<FileDto> Photos) : ICommand;