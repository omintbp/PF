using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.ValueObjects;

namespace PetFamily.Domain.SpeciesManagement.AggregateRoot;

public class Species : Shared.Entity<SpeciesId>
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

    public Result<Breed, Error> GetBreedById(BreedId breedId)
    {
        var breed = _breeds.FirstOrDefault(b => b.Id == breedId);

        if (breed is null)
            return Errors.General.NotFound();

        return breed;
    }

    public UnitResult<Error> DeleteBreedById(BreedId breedId)
    {
        var breedResult = GetBreedById(breedId);

        if (breedResult.IsFailure)
            return breedResult.Error;
        
        _breeds.Remove(breedResult.Value);
        
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> AddBreed(Breed breed)
    {
        if (_breeds.Exists(b => b.Name == breed.Name))
            return Errors.General.AlreadyExist(nameof(breed));
        
        _breeds.Add(breed);
        
        return UnitResult.Success<Error>();
    }

    public void AddBreeds(IEnumerable<Breed> breeds) => _breeds.AddRange(breeds);
}