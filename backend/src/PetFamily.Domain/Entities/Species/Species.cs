using PetFamily.Domain.Entities.Species.Breeds;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Species;

public class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];

    private Species(SpeciesId id)
        : base(id)
    {
    }

    private Species(SpeciesId id, SpeciesName name, List<Breed> breeds)
        : base(id)
    {
        Name = name;
        _breeds = breeds;
    }

    public SpeciesName Name { get; private set; }

    public IReadOnlyList<Breed> Breeds => _breeds;

    public static Species Create(SpeciesId id, SpeciesName name, List<Breed> breeds)
    {
        var species = new Species(id, name, breeds);

        return species;
    }
}