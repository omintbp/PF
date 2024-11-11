using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Species.Application.Commands.DeleteBreed;

public class DeleteBreedCommandHandler : ICommandHandler<DeleteBreedCommand>
{
    private readonly ILogger<DeleteBreedCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteBreedCommand> _validator;
    private readonly IVolunteerContract _volunteerContract;
    private readonly ISpeciesRepository _repository;

    public DeleteBreedCommandHandler(
        ILogger<DeleteBreedCommandHandler> logger,
        [FromKeyedServices(Modules.Species)] IUnitOfWork unitOfWork,
        IValidator<DeleteBreedCommand> validator,
        IVolunteerContract volunteerContract,
        ISpeciesRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _volunteerContract = volunteerContract;
        _repository = repository;
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

        var isBreedActiveResult =
            await _volunteerContract.CheckIfPetExistsByBreedId(command.BreedId, cancellationToken);

        if (isBreedActiveResult.IsFailure)
            return isBreedActiveResult.Error;

        var isBreedActive = isBreedActiveResult.Value;
        if (isBreedActive)
            return Errors.General.ValueIsInvalid(nameof(breedId)).ToErrorList();

        var removeResult = speciesResult.Value.DeleteBreedById(breedId);

        if (removeResult.IsFailure)
            return removeResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Deleted breed: {breedId}", breedId);

        return UnitResult.Success<ErrorList>();
    }
}