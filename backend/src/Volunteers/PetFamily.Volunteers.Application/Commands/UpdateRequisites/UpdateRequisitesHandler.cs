using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.UpdateRequisites;

public sealed class UpdateRequisitesHandler : ICommandHandler<Guid, UpdateRequisitesCommand>
{
    private readonly IVolunteerRepository _repository;
    private readonly IValidator<UpdateRequisitesCommand> _validator;
    private readonly ILogger<UpdateRequisitesCommand> _logger;

    public UpdateRequisitesHandler(
        IVolunteerRepository repository,
        IValidator<UpdateRequisitesCommand> validator,
        ILogger<UpdateRequisitesCommand> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateRequisitesCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var volunteer = volunteerResult.Value;

        var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value);

        var volunteerRequisites = new VolunteerRequisites(requisites);

        volunteer.UpdateRequisites(volunteerRequisites);

        var id = await _repository.Save(volunteer, cancellationToken);

        _logger.LogInformation("Requisites for volunteer {id} updated", id);

        return id;
    }
}