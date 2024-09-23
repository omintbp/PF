using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Application.SpeciesHandlers.Commands.Delete;

public class DeleteSpeciesCommandHandler : ICommandHandler<DeleteSpeciesCommand>
{
    private readonly ILogger<DeleteSpeciesCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteSpeciesCommand> _validator;
    private readonly ISpeciesRepository _repository;

    public DeleteSpeciesCommandHandler(
        ILogger<DeleteSpeciesCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IValidator<DeleteSpeciesCommand> validator,
        ISpeciesRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _repository = repository;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        DeleteSpeciesCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var speciesId = SpeciesId.Create(command.SpeciesId);
        var speciesResult = await _repository.GetById(speciesId, cancellationToken);
        
        if(speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        _repository.Delete(speciesResult.Value);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation($"Deleted Species: {speciesResult.Value.Id}");

        return UnitResult.Success<ErrorList>();
    }
}