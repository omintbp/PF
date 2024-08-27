using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.ValueObjects;

namespace PetFamily.Domain.SpeciesManagement.Entities;

public class Breed : Entity<BreedId>
{
    private Breed(BreedId id)
        : base(id)
    {
    }

    private Breed(BreedId id, BreedName name)
        : base(id)
    {
        Name = name;
    }

    public BreedName Name { get; private set; }

    public static Breed Create(BreedId id, BreedName name)
    {
        var breedName = new Breed(id, name);

        return breedName;
    }
}