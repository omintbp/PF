using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Species.Breeds;

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