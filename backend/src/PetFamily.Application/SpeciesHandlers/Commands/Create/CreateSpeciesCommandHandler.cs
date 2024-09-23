using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.AggregateRoot;
using PetFamily.Domain.SpeciesManagement.ValueObjects;

namespace PetFamily.Application.SpeciesHandlers.Commands.Create;

public class CreateSpeciesCommandHandler : ICommandHandler<Guid, CreateSpeciesCommand>
{
    private readonly ILogger<CreateSpeciesCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateSpeciesCommand> _validator;
    private readonly ISpeciesRepository _repository;

    public CreateSpeciesCommandHandler(
        ILogger<CreateSpeciesCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IValidator<CreateSpeciesCommand> validator,
        ISpeciesRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _repository = repository;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateSpeciesCommand command, 
        CancellationToken cancellationToken)
    {
        var validationResult  = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var speciesId = SpeciesId.NewSpeciesId();

        var name = SpeciesName.Create(command.Name).Value;
        
        var species = new Species(speciesId, name);
        
        var id = await _repository.Add(species, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Created Species: {speciesId}", speciesId);

        return id.Value;
    }
}