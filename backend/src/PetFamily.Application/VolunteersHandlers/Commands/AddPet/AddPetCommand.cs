using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Volunteers;
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
    PetDetailsDto Details,
    Guid SpeciesId,
    Guid BreedId
) : ICommand;