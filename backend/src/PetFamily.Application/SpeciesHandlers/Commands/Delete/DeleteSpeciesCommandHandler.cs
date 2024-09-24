using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.VolunteersHandlers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Application.SpeciesHandlers.Commands.Delete;

public class DeleteSpeciesCommandHandler : ICommandHandler<DeleteSpeciesCommand>
{
    private readonly ILogger<DeleteSpeciesCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteSpeciesCommand> _validator;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ISpeciesRepository _repository;

    public DeleteSpeciesCommandHandler(
        ILogger<DeleteSpeciesCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IValidator<DeleteSpeciesCommand> validator,
        IVolunteerRepository volunteerRepository,
        ISpeciesRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _volunteerRepository = volunteerRepository;
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

        var volunteers = await _volunteerRepository.GetAll(cancellationToken);
        var isSpeciesActive = volunteers.Any(v =>
            v.Pets.Any(p => p.SpeciesDetails.SpeciesId == speciesId));

        if(isSpeciesActive)
            return Errors.General.ValueIsInvalid(nameof(speciesId)).ToErrorList();
        
        _repository.Delete(speciesResult.Value);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Deleted Species: {speciesId}", speciesId);

        return UnitResult.Success<ErrorList>();
    }
}