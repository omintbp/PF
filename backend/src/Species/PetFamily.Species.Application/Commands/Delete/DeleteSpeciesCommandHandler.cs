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

namespace PetFamily.Species.Application.Commands.Delete;

public class DeleteSpeciesCommandHandler : ICommandHandler<DeleteSpeciesCommand>
{
    private readonly ILogger<DeleteSpeciesCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteSpeciesCommand> _validator;
    private readonly IVolunteerContract _volunteerContract;
    private readonly ISpeciesRepository _repository;

    public DeleteSpeciesCommandHandler(
        ILogger<DeleteSpeciesCommandHandler> logger,
        [FromKeyedServices(Modules.Species)] IUnitOfWork unitOfWork,
        IValidator<DeleteSpeciesCommand> validator,
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

        var isSpeciesActiveResult = await _volunteerContract.CheckIfPetExistsBySpeciesId(command.SpeciesId, cancellationToken);

        if (isSpeciesActiveResult.IsFailure)
            return isSpeciesActiveResult.Error;

        var isSpeciesActive = isSpeciesActiveResult.Value;
        if(isSpeciesActive)
            return Errors.General.ValueIsInvalid(nameof(speciesId)).ToErrorList();
        
        _repository.Delete(speciesResult.Value);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Deleted Species: {speciesId}", speciesId);

        return UnitResult.Success<ErrorList>();
    }
}