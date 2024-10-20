using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Domain.AggregateRoot;

public class Volunteer : SharedKernel.Entity<VolunteerId>, ISoftDeletable
{
    private bool _isDeleted = false;

    private readonly List<Pet> _pets = [];

    private Volunteer(VolunteerId id)
        : base(id)
    {
    }

    public Volunteer(
        VolunteerId id,
        FullName fullName,
        EmailAddress email,
        Description description,
        Experience experience,
        PhoneNumber phoneNumber,
        VolunteerRequisites requisites,
        VolunteerSocialNetworks socialNetworks)
        : base(id)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
        Requisites = requisites;
        SocialNetworks = socialNetworks;
    }

    public FullName FullName { get; private set; }

    public EmailAddress Email { get; private set; }

    public Description Description { get; private set; }

    public Experience Experience { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public VolunteerRequisites Requisites { get; private set; }

    public VolunteerSocialNetworks SocialNetworks { get; private set; }

    public IReadOnlyCollection<Pet> Pets => _pets;

    public int GetPetsHomeFoundCount() =>
        _pets.Count(p => p.HelpStatus == HelpStatus.FoundHome);

    public int GetPetsLookingForHomeCount() =>
        _pets.Count(p => p.HelpStatus == HelpStatus.LookingForHome);

    public int GetPetsNeedsHelpCount() =>
        _pets.Count(p => p.HelpStatus == HelpStatus.NeedsHelp);

    public void UpdateMainInfo(
        FullName fullName,
        Description description,
        Experience experience,
        PhoneNumber phoneNumber)
    {
        FullName = fullName;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        var positionResult = SetPetInitialPosition(pet);

        if (positionResult.IsFailure)
            return positionResult.Error;

        _pets.Add(pet);

        return UnitResult.Success<Error>();
    }

    public void UpdateRequisites(VolunteerRequisites requisites) =>
        Requisites = requisites;

    public void UpdateSocialNetworks(VolunteerSocialNetworks socialNetworks) =>
        SocialNetworks = socialNetworks;

    public void Delete()
    {
        _isDeleted = true;

        foreach (var pet in _pets)
        {
            pet.Delete();
        }
    }

    public void Restore()
    {
        _isDeleted = false;

        foreach (var pet in _pets)
        {
            pet.Restore();
        }
    }

    public Result<Pet, Error> GetPetById(PetId petId)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);

        if (pet is null)
            return Errors.General.NotFound(petId.Value);

        return pet;
    }
    
    public UnitResult<Error> MovePet(Pet pet, Position newPosition)
    {
        if (pet.Position == newPosition || _pets.Count == 1)
            return UnitResult.Success<Error>();

        var adjustResult = AdjustPositionIfOutOfRange(newPosition);

        if (adjustResult.IsFailure)
            return adjustResult.Error;

        var moveResult = MovePetBetweenPositions(pet, newPosition);

        if (moveResult.IsFailure)
            return moveResult.Error;

        pet.SetPosition(newPosition);

        return UnitResult.Success<Error>();
    }
    
    private Result<Position, Error> SetPetInitialPosition(Pet pet)
    {
        var position = Position.Create(_pets.Count + 1);

        if (position.IsFailure)
            return position.Error;

        pet.SetPosition(position.Value);

        return position;
    }

    private UnitResult<Error> MovePetBetweenPositions(Pet pet, Position newPosition)
    {
        if (newPosition.Value > pet.Position.Value)
        {
            var petsToMove = _pets.Where(p => p.Position.Value > pet.Position.Value
                                              && p.Position.Value <= newPosition.Value);
            foreach (var petToMove in petsToMove)
            {
                var moveBackwardResult = petToMove.Position.MoveBackward();

                if (moveBackwardResult.IsFailure)
                    return moveBackwardResult.Error;
                
                petToMove.SetPosition(moveBackwardResult.Value);
            }
        }
        else
        {
            var petsToMove = _pets.Where(p => p.Position.Value < pet.Position.Value
                                              && p.Position.Value >= newPosition.Value);
            foreach (var petToMove in petsToMove)
            {
                var moveForwardResult = petToMove.Position.MoveForward();

                if (moveForwardResult.IsFailure)
                    return moveForwardResult.Error;
                
                petToMove.SetPosition(moveForwardResult.Value);
            }
        }

        return UnitResult.Success<Error>();
    }

    private Result<Position, Error> AdjustPositionIfOutOfRange(Position position)
    {
        if (position.Value <= _pets.Count)
            return position;

        var adjustedPositionResult = Position.Create(_pets.Count);

        if (adjustedPositionResult.IsFailure)
            return adjustedPositionResult.Error;

        return adjustedPositionResult.Value;
    }
}