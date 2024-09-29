using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.VolunteersHandlers.Commands.UpdatePet;

public class UpdatePetCommandHandler : ICommandHandler<Guid, UpdatePetCommand>
{
    private readonly ILogger<UpdatePetCommandHandler> _logger;
    private readonly IValidator<UpdatePetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadDbContext _readDbContext;
    private readonly IVolunteerRepository _repository;

    public UpdatePetCommandHandler(
        ILogger<UpdatePetCommandHandler> logger,
        IValidator<UpdatePetCommand> validator,
        IUnitOfWork unitOfWork,
        IReadDbContext readDbContext,
        IVolunteerRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _readDbContext = readDbContext;
        _repository = repository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var volunteer = volunteerResult.Value;
        
        var petId = PetId.Create(command.PetId);

        var petResult = volunteer.GetPetById(petId);
        
        if(petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var pet = petResult.Value;
        
        var isSpeciesExists = _readDbContext.Species.Any(s => s.Id == command.SpeciesId);

        if (isSpeciesExists == false)
            return Errors.General.NotFound(command.SpeciesId).ToErrorList();
        
        var isBreedExists = _readDbContext.Breeds.Any(b => 
            b.Id == command.BreedId && b.SpeciesId == command.SpeciesId);
        
        if (isBreedExists == false)
            return Errors.General.NotFound(command.BreedId).ToErrorList();

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
            command.Details.Birthday).Value;

        var speciesId = SpeciesId.Create(command.SpeciesId);

        var speciesDetails = SpeciesDetails.Create(speciesId, command.BreedId);
        
        pet.Update(
            name,
            description,
            address, 
            phone, 
            command.Status, 
            paymentDetails, 
            petDetails, 
            speciesDetails);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Pet with ID {petId} has been successfully updated.", petId);
        
        return petId.Value;
    }
}