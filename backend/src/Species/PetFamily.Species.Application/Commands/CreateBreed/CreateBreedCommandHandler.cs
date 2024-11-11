using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.Species.Domain.Entities;
using PetFamily.Species.Domain.ValueObjects;

namespace PetFamily.Species.Application.Commands.CreateBreed;

public class CreateBreedCommandHandler : ICommandHandler<Guid, CreateBreedCommand>
{
    private readonly ILogger<CreateBreedCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateBreedCommand> _validator;
    private readonly ISpeciesRepository _repository;

    public CreateBreedCommandHandler(
        ILogger<CreateBreedCommandHandler> logger,
        [FromKeyedServices(Modules.Species)] IUnitOfWork unitOfWork,
        IValidator<CreateBreedCommand> validator,
        ISpeciesRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateBreedCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var speciesId = SpeciesId.Create(command.SpeciesId);

        var speciesResult = await _repository.GetById(speciesId, cancellationToken);

        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var species = speciesResult.Value;

        var isBreedAlreadyExists = species.Breeds.Any(b => b.Name.Value == command.Name);

        if (isBreedAlreadyExists)
            return Errors.General.AlreadyExist(command.Name).ToErrorList();

        var breedId = BreedId.NewBreedId();
        var breedName = BreedName.Create(command.Name).Value;

        var breed = new Breed(breedId, breedName);

        var addBreedResult = species.AddBreed(breed);

        if (addBreedResult.IsFailure)
            return addBreedResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Created Breed {breedName} with id = {breedId} for species {speciesId}",
            breedName,
            breedId,
            speciesId);

        return breedId.Value;
    }
}