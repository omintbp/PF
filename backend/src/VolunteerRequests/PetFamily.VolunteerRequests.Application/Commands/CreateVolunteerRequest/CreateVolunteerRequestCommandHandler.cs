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
using PetFamily.VolunteerRequests.Domain.AggregateRoot;
using PetFamily.VolunteerRequests.Domain.ValueObjects;

namespace PetFamily.VolunteerRequests.Application.Commands.CreateVolunteerRequest;

public class CreateVolunteerRequestCommandHandler : ICommandHandler<Guid, CreateVolunteerRequestCommand>
{
    private readonly ILogger<CreateVolunteerRequestCommandHandler> _logger;
    private readonly IVolunteerRequestsRepository _repository;
    private readonly IValidator<CreateVolunteerRequestCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVolunteerRequestCommandHandler(
        ILogger<CreateVolunteerRequestCommandHandler> logger,
        IVolunteerRequestsRepository repository,
        IValidator<CreateVolunteerRequestCommand> validator,
        [FromKeyedServices(Modules.VolunteerRequests)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerRequestCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var experience = Experience.Create(command.Experience).Value;

        var requisites = command.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value);

        var volunteerInfo = VolunteerInfo.Create(experience, requisites).Value;

        var volunteerRequestId = VolunteerRequestId.NewVolunteerRequestId();

        var volunteerRequestResult = VolunteerRequest.Create(
            volunteerRequestId,
            command.UserId,
            volunteerInfo);

        if (volunteerRequestResult.IsFailure)
            return volunteerRequestResult.Error.ToErrorList();

        var id = await _repository.Add(volunteerRequestResult.Value, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("User {userId} created request {volunteerRequestId} for become volunteer",
            command.UserId,
            id.Value);

        return id.Value;
    }
}