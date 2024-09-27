using PetFamily.Application.Abstractions;
using PetFamily.Domain.PetManagement;

namespace PetFamily.Application.VolunteersHandlers.Commands.UpdatePetStatus;

public record UpdatePetStatusCommand(Guid VolunteerId, Guid PetId, HelpStatus NewStatus) : ICommand;