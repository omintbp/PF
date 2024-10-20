using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs.Shared;
using PetFamily.Core.DTOs.Volunteers;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Commands.UpdatePet;

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