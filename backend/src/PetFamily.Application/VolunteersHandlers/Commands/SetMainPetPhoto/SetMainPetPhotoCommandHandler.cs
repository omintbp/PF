using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Application.VolunteersHandlers.Commands.SetMainPetPhoto;

public class SetMainPetPhotoCommandHandler : ICommandHandler<SetMainPetPhotoCommand>
{
    private readonly ILogger<SetMainPetPhotoCommandHandler> _logger;
    private readonly IVolunteerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SetMainPetPhotoCommandHandler(
        ILogger<SetMainPetPhotoCommandHandler> logger,
        IVolunteerRepository repository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        SetMainPetPhotoCommand command,
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

        var photoId = PetPhotoId.Create(command.PhotoId);

        var setMainPhotoResult = petResult.Value.SetMainPhoto(photoId);

        if (setMainPhotoResult.IsFailure)
            return setMainPhotoResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Set Main Pet {petId} Photo {photoId}", petId, photoId);

        return UnitResult.Success<ErrorList>();
    }
}