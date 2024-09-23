using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.ValueObjects;

namespace PetFamily.Domain.SpeciesManagement.AggregateRoot;

public class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];

    private Species()
    {
    }

    public Species(SpeciesId id, SpeciesName name)
        : base(id)
    {
        Name = name;
    }

    public SpeciesName Name { get; private set; }

    public IReadOnlyList<Breed> Breeds => _breeds;

    public void AddBreed(Breed breed) => _breeds.Add(breed);
    
    public void AddBreeds(IEnumerable<Breed> breeds) => _breeds.AddRange(breeds);
}