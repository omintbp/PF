using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Commands.SoftDeletePet;

public record SoftDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;