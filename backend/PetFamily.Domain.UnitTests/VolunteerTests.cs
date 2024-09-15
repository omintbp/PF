using FluentAssertions;
using PetFamily.Domain.PetManagement;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.UnitTests;

public class VolunteerTests
{
    [Fact]
    public void Add_Pet_With_Empty_Pets_Success_Result()
    {
        // arrange
        var volunteer = CreateVolunteer(0);
        var pet = CreatePet();
        
        // act
        var result = volunteer.AddPet(pet);

        // assert
        var addedPet = volunteer.GetPetById(pet.Id);
        
        result.IsSuccess.Should().BeTrue();
        addedPet.IsSuccess.Should().BeTrue();
        addedPet.Value.Id.Should().Be(pet.Id);
        addedPet.Value.Position.Should().Be(Position.First);
    }

    [Fact]
    public void Add_Pet_With_Not_Empty_Pets_Success_Result()
    {
        // arrange
        const int petsCount = 3;
        var volunteer = CreateVolunteer(petsCount);
        var petToAdd = CreatePet();

        var firstPet = volunteer.Pets.ToList()[0];
        var secondPet = volunteer.Pets.ToList()[1];
        var thirdPet = volunteer.Pets.ToList()[2];
        
        // act
        var result = volunteer.AddPet(petToAdd);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets.Count.Should().Be(petsCount + 1);
        petToAdd.Position.Value.Should().Be(petsCount + 1);
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(2);
        thirdPet.Position.Value.Should().Be(3);
    }
    
    [Fact]
    public void MovePet_With_Single_Pets_Should_Not_Move_Return_Success_Result()
    {
        // arrange
        const int petsCount = 1;
        var volunteer = CreateVolunteer(petsCount);
        var pet = volunteer.Pets.First();
        var newPosition = Position.First;
        
        // act
        var moveResult = volunteer.MovePet(pet, newPosition);
        
        // assert
        moveResult.IsSuccess.Should().BeTrue();
        volunteer.Pets.Count.Should().Be(1);
        pet.Position.Value.Should().Be(Position.First.Value);
    }
    
    [Fact]
    public void MovePet_With_Single_Pets_To_Out_Of_Range_Should_Return_Success_Result()
    {
        // arrange
        const int petsCount = 1;
        var volunteer = CreateVolunteer(petsCount);
        var pet = volunteer.Pets.First();
        var newPosition = Position.Create(3).Value;
        
        // act
        var moveResult = volunteer.MovePet(pet, newPosition);
        
        // assert
        moveResult.IsSuccess.Should().BeTrue();
        volunteer.Pets.Count.Should().Be(1);
        pet.Position.Value.Should().Be(Position.First.Value);
    }
    
    [Fact]
    public void MovePet_Should_Move_Others_Forward_If_New_Position_Is_Lower()
    {
        // arrange
        const int petsCount = 4;
        var volunteer = CreateVolunteer(petsCount);

        var firstPet = volunteer.Pets.ToList()[0];
        var secondPet = volunteer.Pets.ToList()[1];
        var thirdPet = volunteer.Pets.ToList()[2];
        var fourthPet = volunteer.Pets.ToList()[3];
        
        var newPosition = Position.Create(2).Value;
        
        // act
        var moveResult = volunteer.MovePet(fourthPet, newPosition);
        
        // assert
        moveResult.IsSuccess.Should().BeTrue();
        volunteer.Pets.Count.Should().Be(petsCount);
        
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(2);
    }
    
    [Fact]
    public void MovePet_Should_Move_Others_Backward_If_New_Position_Is_Greater()
    {
        // arrange
        const int petsCount = 4;
        var volunteer = CreateVolunteer(petsCount);

        var firstPet = volunteer.Pets.ToList()[0];
        var secondPet = volunteer.Pets.ToList()[1];
        var thirdPet = volunteer.Pets.ToList()[2];
        var fourthPet = volunteer.Pets.ToList()[3];
        
        var newPosition = Position.Create(4).Value;
        
        // act
        var moveResult = volunteer.MovePet(secondPet, newPosition);
        
        // assert
        moveResult.IsSuccess.Should().BeTrue();
        volunteer.Pets.Count.Should().Be(petsCount);
        
        firstPet.Position.Value.Should().Be(1);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        secondPet.Position.Value.Should().Be(4);
    }
    
    [Fact]
    public void Move_Pet_Should_Move_Others_Forward_If_New_Position_Is_First()
    {
        // arrange
        const int petsCount = 4;
        var volunteer = CreateVolunteer(petsCount);

        var firstPet = volunteer.Pets.ToList()[0];
        var secondPet = volunteer.Pets.ToList()[1];
        var thirdPet = volunteer.Pets.ToList()[2];
        var fourthPet = volunteer.Pets.ToList()[3];
        
        var newPosition = Position.First;
        
        // act
        var moveResult = volunteer.MovePet(fourthPet, newPosition);
        
        // assert
        moveResult.IsSuccess.Should().BeTrue();
        volunteer.Pets.Count.Should().Be(petsCount);
        
        fourthPet.Position.Value.Should().Be(1);
        firstPet.Position.Value.Should().Be(2);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
    }
    
    [Fact]
    public void Move_Pet_Should_Move_Others_Backward_If_New_Position_Is_Last()
    {
        // arrange
        const int petsCount = 4;
        var volunteer = CreateVolunteer(petsCount);

        var firstPet = volunteer.Pets.ToList()[0];
        var secondPet = volunteer.Pets.ToList()[1];
        var thirdPet = volunteer.Pets.ToList()[2];
        var fourthPet = volunteer.Pets.ToList()[3];
        
        var newPosition = Position.Create(petsCount).Value;
        
        // act
        var moveResult = volunteer.MovePet(firstPet, newPosition);
        
        // assert
        moveResult.IsSuccess.Should().BeTrue();
        volunteer.Pets.Count.Should().Be(petsCount);
        
        firstPet.Position.Value.Should().Be(4);
        secondPet.Position.Value.Should().Be(1);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
    }
    
    [Fact]
    public void MovePet_Should_Not_Move_To_The_Same_Position()
    {
        // arrange
        const int petsCount = 4;
        var volunteer = CreateVolunteer(petsCount);

        var firstPet = volunteer.Pets.ToList()[0];
        var secondPet = volunteer.Pets.ToList()[1];
        var thirdPet = volunteer.Pets.ToList()[2];
        var fourthPet = volunteer.Pets.ToList()[3];
        
        var newPosition = Position.Create(2).Value;
        
        // act
        var moveResult = volunteer.MovePet(secondPet, newPosition);
        
        // assert
        moveResult.IsSuccess.Should().BeTrue();
        volunteer.Pets.Count.Should().Be(petsCount);
        
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(2);
        thirdPet.Position.Value.Should().Be(3);
        fourthPet.Position.Value.Should().Be(4);
    }

    private Volunteer CreateVolunteer(int petsCount)
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        var fullName = FullName.Create("test", "test", "test").Value;
        var email = EmailAddress.Create("email@test.com").Value;
        var description = Description.Create("test").Value;
        var experience = Experience.Create(10).Value;
        var phoneNumber = PhoneNumber.Create("79533412323").Value;
        var requisites = new VolunteerRequisites(new List<Requisite>());
        var socialNetworks = new VolunteerSocialNetworks(new List<SocialNetwork>());

        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            email,
            description,
            experience,
            phoneNumber,
            requisites,
            socialNetworks);

        var pets = Enumerable.Range(0, petsCount)
            .Select(x => CreatePet())
            .ToList();
        
        foreach (var pet in pets)
        {
            volunteer.AddPet(pet);    
        }

        return volunteer;
    }

    private Pet CreatePet()
    {
        var name = PetName.Create("test").Value;
        var description = Description.Create("test").Value;

        var address = Address.Create(
            "test",
            "test",
            "test",
            "test",
            "test").Value;

        var phone = PhoneNumber.Create("7955545433").Value;

        var paymentDetails = new PaymentDetails(new List<Requisite>());

        var petDetails = PetDetails.Create(
            1.0,
            1.0,
            false,
            false,
            "command.Details.Color",
            "command.Details.HealthInfo",
            DateTime.Today).Value;

        var petId = PetId.NewPetId();

        var pet = new Pet(
            petId,
            name,
            description,
            address,
            phone,
            HelpStatus.FoundHome,
            DateTime.Now,
            paymentDetails,
            petDetails,
            SpeciesDetails.None);

        return pet;
    }
}