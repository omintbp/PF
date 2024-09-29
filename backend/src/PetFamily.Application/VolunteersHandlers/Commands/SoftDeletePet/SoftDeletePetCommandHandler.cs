using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Messaging;
using PetFamily.Application.VolunteersHandlers.Commands.DeletePet;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;
using FileInfo = PetFamily.Application.Providers.FileInfo;

namespace PetFamily.Application.VolunteersHandlers.Commands.SoftDeletePet;

public class SoftDeletePetCommandHandler : ICommandHandler<SoftDeletePetCommand>
{
    private readonly ILogger<SoftDeletePetCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRepository _repository;

    public SoftDeletePetCommandHandler(
        ILogger<SoftDeletePetCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IVolunteerRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        SoftDeletePetCommand command,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);
        var petResult = volunteerResult.Value.GetPetById(petId);

        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        petResult.Value.Delete();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Soft deleted Pet with ID: {petId}", petId);

        return UnitResult.Success<ErrorList>();
    }
}