using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.SpeciesHandlers.Commands.Delete;
using PetFamily.Application.VolunteersHandlers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Application.SpeciesHandlers.Commands.DeleteBreed;

public class DeleteBreedCommandHandler : ICommandHandler<DeleteBreedCommand>
{
    private readonly ILogger<DeleteBreedCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteBreedCommand> _validator;
    private readonly ISpeciesRepository _repository;
    private readonly IVolunteerRepository _volunteerRepository;

    public DeleteBreedCommandHandler(
        ILogger<DeleteBreedCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IValidator<DeleteBreedCommand> validator,
        ISpeciesRepository repository,
        IVolunteerRepository volunteerRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _repository = repository;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        DeleteBreedCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var speciesId = SpeciesId.Create(command.SpeciesId);
        var speciesResult = await _repository.GetById(speciesId, cancellationToken);

        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var breedId = BreedId.Create(command.BreedId);

        var volunteers = await _volunteerRepository.GetAll(cancellationToken);
        var isBreedActive = volunteers.Any(v =>
            v.Pets.Any(p => p.SpeciesDetails.BreedId == command.BreedId));

        if(isBreedActive)
            return Errors.General.ValueIsInvalid(nameof(breedId)).ToErrorList();
        
        var removeResult = speciesResult.Value.DeleteBreedById(breedId);

        if (removeResult.IsFailure)
            return removeResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Deleted breed: {breedId}", breedId);

        return UnitResult.Success<ErrorList>();
    }
}