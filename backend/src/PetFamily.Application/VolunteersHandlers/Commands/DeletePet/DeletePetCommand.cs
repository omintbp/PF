using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersHandlers.Commands.DeletePet;

public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;