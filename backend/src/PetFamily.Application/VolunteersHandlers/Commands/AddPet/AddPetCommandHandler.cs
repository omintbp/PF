using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.VolunteersHandlers.Commands.AddPet;

public class AddPetCommandHandler : ICommandHandler<Guid, AddPetCommand>
{
    private readonly ILogger<AddPetCommand> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRepository _repository;
    private readonly IValidator<AddPetCommand> _validator;

    public AddPetCommandHandler(
        ILogger<AddPetCommand> logger,
        IUnitOfWork unitOfWork,
        IVolunteerRepository repository,
        IValidator<AddPetCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();
        
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
            command.Details.Weight,
            command.Details.IsCastrated,
            command.Details.IsVaccinated,
            command.Details.Color,
            command.Details.HealthInfo,
            command.Details.Birthday).Value;

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
            SpeciesDetails.None);

        volunteer.AddPet(pet);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Pet {petId} added to volunteer {volunteerId}", petId.Value, volunteerId.Value);

        return petId.Value;
    }
}