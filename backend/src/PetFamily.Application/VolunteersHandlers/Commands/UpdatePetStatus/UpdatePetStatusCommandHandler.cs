using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Application.VolunteersHandlers.Commands.UpdatePetStatus;

public class UpdatePetStatusCommandHandler : ICommandHandler<Guid, UpdatePetStatusCommand>
{
    private readonly ILogger<UpdatePetStatusCommandHandler> _logger;
    private readonly IVolunteerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdatePetStatusCommand> _validator;

    public UpdatePetStatusCommandHandler(
        ILogger<UpdatePetStatusCommandHandler> logger,
        IVolunteerRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<UpdatePetStatusCommand> validator)
    {
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetStatusCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);
        var petResult = volunteerResult.Value.GetPetById(petId);

        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var pet = petResult.Value;

        pet.UpdateStatus(command.NewStatus);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Pet {petId} status updated to {newStatus}", petId, command.NewStatus);

        return petId.Value;
    }
}