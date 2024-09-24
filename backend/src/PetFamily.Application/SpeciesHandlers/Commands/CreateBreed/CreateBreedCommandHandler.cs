using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.ValueObjects;

namespace PetFamily.Application.SpeciesHandlers.Commands.CreateBreed;

public class CreateBreedCommandHandler : ICommandHandler<Guid, CreateBreedCommand>
{
    private readonly ILogger<CreateBreedCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateBreedCommand> _validator;
    private readonly ISpeciesRepository _repository;

    public CreateBreedCommandHandler(
        ILogger<CreateBreedCommandHandler> logger,
        IUnitOfWork unitOfWork,
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

        var breedId = BreedId.NewBreedId();
        var breedName = BreedName.Create(command.Name).Value;

        var breed = new Breed(breedId, breedName);

        var addBreedResult = species.AddBreed(breed);
        
        if(addBreedResult.IsFailure)
            return addBreedResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Created Breed {breedName} with id = {breedId} for species {speciesId}",
            breedName,
            breedId,
            speciesId);

        return breedId.Value;
    }
}