using PetFamily.Application.Abstractions;
using PetFamily.Application.SharedDTOs;
using PetFamily.Domain.PetManagement;

namespace PetFamily.Application.VolunteersHandlers.Commands.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string Name,
    string Description,
    AddressDto Address,
    HelpStatus Status,
    string Phone,
    IEnumerable<RequisiteDto> Requisites,
    PetDetailsDto Details
) : ICommand;