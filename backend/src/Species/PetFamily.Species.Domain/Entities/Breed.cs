using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.Species.Domain.ValueObjects;

namespace PetFamily.Species.Domain.Entities;

public class Breed : Entity<BreedId>
{
    private Breed(BreedId id)
        : base(id)
    {
    }

    public Breed(BreedId id, BreedName name)
        : base(id)
    {
        Name = name;
    }

    public BreedName Name { get; private set; }
}