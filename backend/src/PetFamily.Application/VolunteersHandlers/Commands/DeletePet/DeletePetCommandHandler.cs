using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Messaging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;
using FileInfo = PetFamily.Application.Providers.FileInfo;

namespace PetFamily.Application.VolunteersHandlers.Commands.DeletePet;

public class DeletePetCommandHandler : ICommandHandler<DeletePetCommand>
{
    private const string PHOTOS_BUCKET_NAME = "photos";
    
    private readonly ILogger<DeletePetCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRepository _repository;
    private readonly IPetRepository _petRepository;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _queue;

    public DeletePetCommandHandler(
        ILogger<DeletePetCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IVolunteerRepository repository,
        IPetRepository petRepository,
        IMessageQueue<IEnumerable<FileInfo>> queue)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _petRepository = petRepository;
        _queue = queue;
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
        
        if(petResult.IsFailure)
            return petResult.Error.ToErrorList();
        
        _petRepository.Delete(petResult.Value);

        List<FileInfo> filesInfo = [];
        foreach (var photo in petResult.Value.Photos)
        {
            var fileInfo = new FileInfo(FilePath.Create(photo.Path).Value, PHOTOS_BUCKET_NAME);
            filesInfo.Add(fileInfo);
        }
        
        await _queue.WriteAsync(filesInfo, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Deleted Pet with ID: {petId}", petId);
        
        return UnitResult.Success<ErrorList>();
    }
}