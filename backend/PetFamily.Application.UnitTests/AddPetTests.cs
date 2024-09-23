using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetFamily.Application.Database;
using PetFamily.Application.SharedDTOs;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.Commands.AddPet;
using PetFamily.Domain.PetManagement;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace PetFamily.Application.UnitTests;

public class AddPetTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IVolunteerRepository> _volunteerRepositoryMock;
    private readonly Mock<ILogger<AddPetCommand>> _loggerMock;
    private readonly Mock<IValidator<AddPetCommand>> _validatorMock;

    public AddPetTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _volunteerRepositoryMock = new Mock<IVolunteerRepository>();
        _loggerMock = new Mock<ILogger<AddPetCommand>>();
        _validatorMock = new Mock<IValidator<AddPetCommand>>();
    }

    [Fact]
    public async Task Handle_Should_Add_Pets_To_Volunteer()
    {
        // arrange
        var ct = CancellationToken.None;
        var volunteer = CreateVolunteer(0);
        var command = CreateValidAddPetCommand(volunteer.Id);

        _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);
        _volunteerRepositoryMock.Setup(r => r.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(volunteer);
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetCommand>(), ct))
            .ReturnsAsync(new ValidationResult());

        var addPetHandler = new AddPetCommandHandler(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _volunteerRepositoryMock.Object,
            _validatorMock.Object);

        // act
        var handleResult = await addPetHandler.Handle(command, ct);

        // assert
        var addedPet = volunteer.GetPetById(PetId.Create(handleResult.Value));

        handleResult.IsSuccess.Should().BeTrue();
        volunteer.Pets.Count.Should().Be(1);
        addedPet.IsSuccess.Should().BeTrue();
        addedPet.Value.Id.Value.Should().Be(handleResult.Value);
    }

    [Fact]
    public async Task Handle_Should_Add_Pets_To_Volunteer_If_Pets_Not_Empty()
    {
        // arrange
        const int petsCount = 10;

        var ct = CancellationToken.None;
        var volunteer = CreateVolunteer(petsCount);
        var command = CreateValidAddPetCommand(volunteer.Id);

        _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);
        _volunteerRepositoryMock.Setup(r => r.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(volunteer);
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetCommand>(), ct))
            .ReturnsAsync(new ValidationResult());

        var addPetHandler = new AddPetCommandHandler(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _volunteerRepositoryMock.Object,
            _validatorMock.Object);

        // act
        var handleResult = await addPetHandler.Handle(command, ct);

        // assert
        var addedPet = volunteer.GetPetById(PetId.Create(handleResult.Value));

        handleResult.IsSuccess.Should().BeTrue();
        volunteer.Pets.Count.Should().Be(petsCount + 1);
        addedPet.IsSuccess.Should().BeTrue();
        addedPet.Value.Id.Value.Should().Be(handleResult.Value);
    }

    [Fact]
    public async Task Handle_Should_Return_Error_If_Volunteer_Does_Not_Exist()
    {
        // arrange
        var ct = CancellationToken.None;
        var volunteer = CreateVolunteer(0);
        var command = CreateValidAddPetCommand(volunteer.Id);

        _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);
        _volunteerRepositoryMock.Setup(r => r.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Errors.General.NotFound());
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetCommand>(), ct))
            .ReturnsAsync(new ValidationResult());

        var addPetHandler = new AddPetCommandHandler(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _volunteerRepositoryMock.Object,
            _validatorMock.Object);

        // act
        var handleResult = await addPetHandler.Handle(command, ct);

        // assert
        handleResult.IsFailure.Should().BeTrue();
        handleResult.Error.First().Type.Should().Be(ErrorType.NotFound);
        volunteer.Pets.Count.Should().Be(0);
    }

    [Fact]
    public async Task Handle_Should_Return_Validation_Error_If_Command_Not_Valid()
    {
        // arrange
        var ct = CancellationToken.None;
        var volunteer = CreateVolunteer(0);
        var command = CreateNotValidAddPetCommand(volunteer.Id);

        _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);
        _volunteerRepositoryMock.Setup(r => r.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(volunteer);
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetCommand>(), ct))
            .ReturnsAsync(new ValidationResult(
            [
                new ValidationFailure("phone.number",
                    Errors.General.ValueIsInvalid().Serialize())
            ]));

        var addPetHandler = new AddPetCommandHandler(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _volunteerRepositoryMock.Object,
            _validatorMock.Object);

        // act
        var handleResult = await addPetHandler.Handle(command, ct);

        // assert
        handleResult.IsFailure.Should().BeTrue();
        handleResult.Error.First().Type.Should().Be(ErrorType.Validation);
        volunteer.Pets.Count.Should().Be(0);
    }

    private AddPetCommand CreateValidAddPetCommand(VolunteerId volunteerId)
    {
        var name = "test";
        var description = "test";
        var phone = "842343294234";
        var address = new AddressDto("asd", "asd", "asd", "asd", "asd");
        var requisites = new List<RequisiteDto>();
        var petDetails = new PetDetailsDto(
            1.0,
            1.0,
            false,
            false,
            "asd",
            "asd",
            DateTime.Today);

        return new AddPetCommand(
            volunteerId.Value,
            name,
            description,
            address,
            HelpStatus.FoundHome,
            phone,
            requisites, petDetails);
    }

    private AddPetCommand CreateNotValidAddPetCommand(VolunteerId volunteerId)
    {
        var name = "test";
        var description = "test";
        var phone = "jdasjdkasj@adsdkjaskd.ru";
        var address = new AddressDto("asd", "asd", "asd", "asd", "asd");
        var requisites = new List<RequisiteDto>();
        var petDetails = new PetDetailsDto(
            1.0,
            1.0,
            false,
            false,
            "asd",
            "asd",
            DateTime.Today);

        return new AddPetCommand(
            volunteerId.Value,
            name,
            description,
            address,
            HelpStatus.FoundHome,
            phone,
            requisites, petDetails);
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