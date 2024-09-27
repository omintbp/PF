using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersHandlers.Commands.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;