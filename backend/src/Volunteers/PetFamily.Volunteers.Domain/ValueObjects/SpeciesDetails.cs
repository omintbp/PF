using PetFamily.SharedKernel.IDs;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public record SpeciesDetails
{
    private SpeciesDetails(SpeciesId speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public SpeciesId SpeciesId { get; }

    public Guid BreedId { get; }

    public static SpeciesDetails None => SpeciesDetails.Create(SpeciesId.Empty(), Guid.Empty);

    public static SpeciesDetails Create(SpeciesId speciesId, Guid breedId)
    {
        var details = new SpeciesDetails(speciesId, breedId);

        return details;
    }
}