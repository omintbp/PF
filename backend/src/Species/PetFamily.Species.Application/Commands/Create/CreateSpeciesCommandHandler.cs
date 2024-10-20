using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.Species.Domain.ValueObjects;

namespace PetFamily.Species.Application.Commands.Create;

public class CreateSpeciesCommandHandler : ICommandHandler<Guid, CreateSpeciesCommand>
{
    private readonly ILogger<CreateSpeciesCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateSpeciesCommand> _validator;
    private readonly IReadDbContext _readDbContext;
    private readonly ISpeciesRepository _repository;

    public CreateSpeciesCommandHandler(
        ILogger<CreateSpeciesCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IValidator<CreateSpeciesCommand> validator,
        IReadDbContext readDbContext,
        ISpeciesRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _readDbContext = readDbContext;
        _repository = repository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateSpeciesCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var isNameAlreadyUsed = _readDbContext.Species.Any(s => s.SpeciesName == command.Name);

        if (isNameAlreadyUsed)
            return Errors.General.AlreadyExist(command.Name).ToErrorList();

        var speciesId = SpeciesId.NewSpeciesId();

        var name = SpeciesName.Create(command.Name).Value;

        var species = new Domain.AggregateRoot.Species(speciesId, name);

        var id = await _repository.Add(species, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Created Species: {speciesId}", speciesId);

        return id.Value;
    }
}