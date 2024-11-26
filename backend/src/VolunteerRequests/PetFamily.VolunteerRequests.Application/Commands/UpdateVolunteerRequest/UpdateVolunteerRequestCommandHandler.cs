using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerRequests.Domain.ValueObjects;

namespace PetFamily.VolunteerRequests.Application.Commands.UpdateVolunteerRequest;

public class UpdateVolunteerRequestCommandHandler
    : ICommandHandler<Guid, UpdateVolunteerRequestCommand>
{
    private readonly ILogger<UpdateVolunteerRequestCommand> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateVolunteerRequestCommand> _validator;
    private readonly IVolunteerRequestsRepository _repository;

    public UpdateVolunteerRequestCommandHandler(
        ILogger<UpdateVolunteerRequestCommand> logger,
        [FromKeyedServices(Modules.VolunteerRequests)]
        IUnitOfWork unitOfWork,
        IValidator<UpdateVolunteerRequestCommand> validator,
        IVolunteerRequestsRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerRequestCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var experience = Experience.Create(command.Experience).Value;

        var requisites = command.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value);

        var volunteerInfo = VolunteerInfo.Create(experience, requisites).Value;

        var volunteerRequestId = VolunteerRequestId.Create(command.VolunteerRequestId);

        var volunteerRequestResult = await _repository.GetById(volunteerRequestId, cancellationToken);
        if (volunteerRequestResult.IsFailure)
            return volunteerRequestResult.Error.ToErrorList();

        var updateResult = volunteerRequestResult.Value.Update(volunteerInfo);
        if (updateResult.IsFailure)
            return updateResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Volunteer request {volunteerRequestId} has been successfully updated",
            volunteerRequestId.Value);

        return volunteerRequestId.Value;
    }
}