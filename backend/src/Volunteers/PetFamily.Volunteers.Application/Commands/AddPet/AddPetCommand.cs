using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs.Shared;
using PetFamily.Core.DTOs.Volunteers;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Commands.AddPet;

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