using PetFamily.Core.DTOs.Shared;
using PetFamily.Core.DTOs.Volunteers;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Commands.AddPet;

namespace PetFamily.Volunteers.Presentation.Volunteers.Requests;

public record AddPetRequest(
    string Name,
    string Description,
    AddressDto Address,
    HelpStatus Status,
    string Phone,
    IEnumerable<RequisiteDto> Requisites,
    PetDetailsDto Details,
    Guid SpeciesId,
    Guid BreedId)
{
    public AddPetCommand ToCommand(Guid volunteerId) =>
        new AddPetCommand(
            volunteerId,
            Name,
            Description,
            Address,
            Status,
            Phone,
            Requisites,
            Details,
            SpeciesId,
            BreedId);
}