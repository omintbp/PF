using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.ValueObjects;

namespace PetFamily.Domain.SpeciesManagement.AggregateRoot;

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

    public void AddBreed(Breed breed) => _breeds.Add(breed);
}