using PetFamily.Application.SharedDTOs;

namespace PetFamily.Application.Volunteers.AddPetPhotos;

public record AddPetPhotosCommand(Guid VolunteerId, Guid PetId, IEnumerable<FileDto> Photos);