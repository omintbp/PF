using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Species.Contracts;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.AddPet;

public class AddPetCommandHandler : ICommandHandler<Guid, AddPetCommand>
{
    private readonly ILogger<AddPetCommand> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRepository _repository;
    private readonly ISpeciesContract _speciesContract;
    private readonly IValidator<AddPetCommand> _validator;

    public AddPetCommandHandler(
        ILogger<AddPetCommand> logger,
        [FromKeyedServices(Modules.Pets)] IUnitOfWork unitOfWork,
        IVolunteerRepository repository,
        ISpeciesContract speciesContract,
        IValidator<AddPetCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _speciesContract = speciesContract;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var isSpeciesExistsResult = await _speciesContract.CheckIfSpeciesExists(command.SpeciesId, cancellationToken);
        if (isSpeciesExistsResult.IsFailure)
            return isSpeciesExistsResult.Error;

        var isSpeciesExists = isSpeciesExistsResult.Value;
        if (isSpeciesExists == false)
            return Errors.General.NotFound(command.SpeciesId).ToErrorList();
        
        var isBreedExistsResult = await _speciesContract.CheckIfBreedExists(
            command.SpeciesId, 
            command.BreedId, 
            cancellationToken);
        
        if (isBreedExistsResult.IsFailure)
            return isBreedExistsResult.Error;
        
        var isBreedExists = isBreedExistsResult.Value;
        if (isBreedExists == false)
            return Errors.General.NotFound(command.BreedId).ToErrorList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(volunteerId.Value).ToErrorList();

        var volunteer = volunteerResult.Value;

        var name = PetName.Create(command.Name).Value;

        var description = Description.Create(command.Description).Value;

        var address = Address.Create(
            command.Address.Country,
            command.Address.City,
            command.Address.Street,
            command.Address.House,
            command.Address.Flat).Value;

        var phone = PhoneNumber.Create(command.Phone).Value;

        var requisites = command.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value);
        var paymentDetails = new PaymentDetails(requisites);

        var petDetails = PetDetails.Create(
            command.Details.Weight,
            command.Details.Height,
            command.Details.IsCastrated,
            command.Details.IsVaccinated,
            command.Details.Color,
            command.Details.HealthInfo,
            command.Details.BirthdayDate).Value;

        var speciesId = SpeciesId.Create(command.SpeciesId);

        var speciesDetails = SpeciesDetails.Create(speciesId, command.BreedId);

        var petId = PetId.NewPetId();

        var pet = new Pet(
            petId,
            name,
            description,
            address,
            phone,
            command.Status,
            DateTime.Now,
            paymentDetails,
            petDetails,
            speciesDetails);

        volunteer.AddPet(pet);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Pet {petId} added to volunteer {volunteerId}", petId.Value, volunteerId.Value);

        return petId.Value;
    }
}