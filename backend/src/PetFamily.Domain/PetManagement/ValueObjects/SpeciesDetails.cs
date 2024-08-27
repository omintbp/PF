using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record SpeciesDetails
{
    private SpeciesDetails(SpeciesId speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }
    
    public SpeciesId SpeciesId { get; }
    
    public Guid BreedId { get; }

    public static SpeciesDetails Create(SpeciesId speciesId, Guid breedId)
    {
        var details = new SpeciesDetails(speciesId, breedId);
        
        return details;
    }
    
}