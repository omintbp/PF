using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs.Shared;

namespace PetFamily.Application.VolunteersHandlers.Commands.AddPetPhotos;

public record AddPetPhotosCommand(Guid VolunteerId, Guid PetId, IEnumerable<FileDto> Photos) : ICommand;