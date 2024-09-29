using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Volunteers;
using PetFamily.Application.VolunteersHandlers.Commands.UpdatePet;
using PetFamily.Domain.PetManagement;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.API.Controllers.Volunteers.Requests;

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