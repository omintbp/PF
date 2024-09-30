using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Species;
using PetFamily.Domain.PetManagement;

namespace PetFamily.Application.DTOs.Volunteers;

public class PetDto
{
    public Guid Id { get; set; }

    public Guid VolunteerId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public AddressDto Address { get; set; }
    public HelpStatus Status { get; set; }
    public string Phone { get; set; }
    public IEnumerable<RequisiteDto> Requisites { get; set; }

    public List<PetPhotoDto> Photos { get; set; }

    public PetDetailsDto Details { get; set; }

    public SpeciesDto Species { get; set; }

    public BreedDto Breed { get; set; }

    public PetDto()
    {
    }
}