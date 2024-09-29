using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersHandlers.Commands.SoftDeletePet;

public record SoftDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;