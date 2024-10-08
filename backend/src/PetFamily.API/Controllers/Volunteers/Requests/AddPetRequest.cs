using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Volunteers;
using PetFamily.Application.VolunteersHandlers.Commands.AddPet;
using PetFamily.Domain.PetManagement;

namespace PetFamily.API.Controllers.Volunteers.Requests;

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