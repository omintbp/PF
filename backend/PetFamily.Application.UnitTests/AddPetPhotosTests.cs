using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Moq;
using PetFamily.Application.Database;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Application.SharedDTOs;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.Commands.AddPetPhotos;
using PetFamily.Domain.PetManagement;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;
using FileInfo = PetFamily.Application.Providers.FileInfo;


namespace PetFamily.Application.UnitTests;

public class AddPetPhotosTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IFileProvider> _fileProviderMock = new();
    private readonly Mock<IVolunteerRepository> _volunteerRepositoryMock = new();
    private readonly Mock<ILogger<AddPetPhotosCommand>> _loggerMock = new();
    private readonly Mock<IValidator<AddPetPhotosCommand>> _validatorMock = new();
    private readonly Mock<IMessageQueue<IEnumerable<FileInfo>>> _queue = new();

    [Fact]
    public async Task Handle_Should_Return_Error_If_Volunteer_Not_Found()
    {
        // arrange
        const int petCount = 1;
        const int filesCount = 3;

        var volunteer = CreateVolunteer(petCount);
        var pet = volunteer.Pets.First();

        var ct = new CancellationTokenSource().Token;

        var files = CreateFileDtos(filesCount);

        var command = new AddPetPhotosCommand(volunteer.Id.Value, pet.Id.Value, files);

        _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);

        _volunteerRepositoryMock.Setup(r => r.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Errors.General.NotFound());

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetPhotosCommand>(), ct))
            .ReturnsAsync(new ValidationResult());

        _fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<IEnumerable<FileData>>(), ct))
            .ReturnsAsync(new List<FilePath>());

        var handler = new AddPetPhotosCommandHandler(
            _fileProviderMock.Object,
            _volunteerRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object,
            _validatorMock.Object,
            _queue.Object);

        // act
        var handleResult = await handler.Handle(command, ct);

        // assert
        handleResult.IsFailure.Should().BeTrue();
        handleResult.Error.First().Type.Should().Be(ErrorType.NotFound);
        pet.Photos.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Return_Error_If_Pet_Not_Found()
    {
        // arrange
        const int petCount = 1;
        const int filesCount = 3;

        var volunteer = CreateVolunteer(petCount);
        var pet = volunteer.Pets.First();

        var ct = new CancellationTokenSource().Token;

        var files = CreateFileDtos(filesCount);

        var invalidPetId = Guid.Empty;

        var command = new AddPetPhotosCommand(volunteer.Id.Value, invalidPetId, files);

        _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);

        _volunteerRepositoryMock.Setup(r => r.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(volunteer);

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetPhotosCommand>(), ct))
            .ReturnsAsync(new ValidationResult());

        _fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<IEnumerable<FileData>>(), ct))
            .ReturnsAsync(new List<FilePath>());

        var handler = new AddPetPhotosCommandHandler(
            _fileProviderMock.Object,
            _volunteerRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object,
            _validatorMock.Object,
            _queue.Object);

        // act
        var handleResult = await handler.Handle(command, ct);

        // assert
        handleResult.IsFailure.Should().BeTrue();
        handleResult.Error.First().Type.Should().Be(ErrorType.NotFound);
        pet.Photos.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Return_Error_If_Command_Invalid()
    {
        // arrange
        const int petCount = 1;
        const int filesCount = 3;

        var volunteer = CreateVolunteer(petCount);
        var pet = volunteer.Pets.First();

        var ct = new CancellationTokenSource().Token;

        var files = CreateFileDtos(filesCount);

        var command = new AddPetPhotosCommand(volunteer.Id.Value, pet.Id.Value, files);

        _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);

        _volunteerRepositoryMock.Setup(r => r.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(volunteer);

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetPhotosCommand>(), ct))
            .ReturnsAsync(new ValidationResult(
            [
                new ValidationFailure(
                    "fileName",
                    Errors.General.ValueIsInvalid().Serialize())
            ]));

        _fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<IEnumerable<FileData>>(), ct))
            .ReturnsAsync(new List<FilePath>());

        var handler = new AddPetPhotosCommandHandler(
            _fileProviderMock.Object,
            _volunteerRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object,
            _validatorMock.Object,
            _queue.Object);

        // act
        var handleResult = await handler.Handle(command, ct);

        // assert
        handleResult.IsFailure.Should().BeTrue();
        handleResult.Error.First().Type.Should().Be(ErrorType.Validation);
        pet.Photos.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Upload_Files_To_Pet_Successfully()
    {
        // arrange
        const int petCount = 1;
        const int filesCount = 3;

        var volunteer = CreateVolunteer(petCount);
        var pet = volunteer.Pets.First();

        var ct = new CancellationTokenSource().Token;

        var files = CreateFileDtos(filesCount);

        var command = new AddPetPhotosCommand(volunteer.Id.Value, pet.Id.Value, files);

        _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);

        _volunteerRepositoryMock.Setup(r => r.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(volunteer);

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetPhotosCommand>(), ct))
            .ReturnsAsync(new ValidationResult());

        _fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<IEnumerable<FileData>>(), ct))
            .ReturnsAsync(Enumerable.Range(1, filesCount)
                .Select(i => FilePath.Create(Guid.NewGuid(), ".png").Value)
                .ToList());

        var handler = new AddPetPhotosCommandHandler(
            _fileProviderMock.Object,
            _volunteerRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object,
            _validatorMock.Object,
            _queue.Object);

        // act
        var handleResult = await handler.Handle(command, ct);

        // assert
        handleResult.IsSuccess.Should().BeTrue();
        handleResult.Value.Should().Be(volunteer.Id.Value);
        pet.Photos.Count.Should().Be(filesCount);
    }

    private IEnumerable<FileDto> CreateFileDtos(int count)
        => Enumerable.Range(1, count)
            .Select(i => new FileDto(Stream.Null, $"file{i}.png"));

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