using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.Volunteers.Application.Providers;
using FileInfo = PetFamily.Volunteers.Application.Providers.FileInfo;

namespace PetFamily.Volunteers.Application.Commands.DeletePet;

public class DeletePetCommandHandler : ICommandHandler<DeletePetCommand>
{
    private const string PHOTOS_BUCKET_NAME = "photos";

    private readonly ILogger<DeletePetCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRepository _repository;
    private readonly IPetRepository _petRepository;
    private readonly IFileProvider _fileProvider;

    public DeletePetCommandHandler(
        ILogger<DeletePetCommandHandler> logger,
        [FromKeyedServices(Modules.Pets)] IUnitOfWork unitOfWork,
        IVolunteerRepository repository,
        IPetRepository petRepository,
        IFileProvider fileProvider)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _petRepository = petRepository;
        _fileProvider = fileProvider;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        DeletePetCommand command,
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

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        
        _petRepository.Delete(petResult.Value);

        List<FileInfo> filesInfo = [];
        foreach (var photo in petResult.Value.Photos)
        {
            var fileInfo = new FileInfo(photo.FilePath, PHOTOS_BUCKET_NAME);
            filesInfo.Add(fileInfo);
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);

        var deleteResult = await _fileProvider.DeleteFiles(filesInfo, cancellationToken);

        if (deleteResult.IsFailure)
        {
            transaction.Rollback();
            return deleteResult.Error;
        }
        
        transaction.Commit();
        
        _logger.LogInformation("Deleted Pet with ID: {petId}", petId);

        return UnitResult.Success<ErrorList>();
    }
}