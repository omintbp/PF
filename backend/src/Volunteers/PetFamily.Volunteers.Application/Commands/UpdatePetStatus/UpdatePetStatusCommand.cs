using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Commands.UpdatePetStatus;

public record UpdatePetStatusCommand(Guid VolunteerId, Guid PetId, HelpStatus NewStatus) : ICommand;