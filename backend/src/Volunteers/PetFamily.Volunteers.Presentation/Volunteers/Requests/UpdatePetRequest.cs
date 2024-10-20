using PetFamily.Core.DTOs.Shared;
using PetFamily.Core.DTOs.Volunteers;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Commands.UpdatePet;

namespace PetFamily.Volunteers.Presentation.Volunteers.Requests;

public record UpdatePetRequest(
    string Name,
    string Description,
    AddressDto Address,
    string Phone,
    HelpStatus Status,
    PetDetailsDto Details,
    IEnumerable<RequisiteDto> Requisites,
    Guid SpeciesId,
    Guid BreedId)
{
    public UpdatePetCommand ToCommand(Guid volunteerId, Guid petId)
        => new(
            volunteerId,
            petId,
            Name,
            Description,
            Address,
            Phone,
            Status,
            Requisites,
            Details,
            SpeciesId,
            BreedId);
}