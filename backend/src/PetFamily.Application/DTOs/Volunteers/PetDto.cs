using PetFamily.Application.DTOs.Shared;
using PetFamily.Domain.PetManagement;

namespace PetFamily.Application.DTOs.Volunteers;

public class PetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public AddressDto Address { get; set; }
    public HelpStatus Status { get; set; }
    public string Phone { get; set; }
    public IEnumerable<RequisiteDto> Requisites { get; set; }

    public PetDetailsDto Details { get; set; }
    public Guid SpeciesId { get; set; }
    public Guid BreedId { get; set; }

    public PetDto()
    {
    }
}