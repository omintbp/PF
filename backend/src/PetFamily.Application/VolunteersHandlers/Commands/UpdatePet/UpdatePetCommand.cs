using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Volunteers;
using PetFamily.Domain.PetManagement;

namespace PetFamily.Application.VolunteersHandlers.Commands.UpdatePet;

public record UpdatePetCommand(
    Guid VolunteerId,
    Guid PetId,
    string Name,
    string Description,
    AddressDto Address,
    string Phone,
    HelpStatus Status,
    IEnumerable<RequisiteDto> Requisites,
    PetDetailsDto Details,
    Guid SpeciesId,
    Guid BreedId) : ICommand;