using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.VolunteerRequests.Domain.ValueObjects;

namespace PetFamily.VolunteerRequests.Application.Commands.SendVolunteerRequestToRevision;

public class SendVolunteerRequestToRevisionCommandHandler
    : ICommandHandler<Guid, SendVolunteerRequestToRevisionCommand>
{
    private readonly ILogger<SendVolunteerRequestToRevisionCommandHandler> _logger;
    private readonly IVolunteerRequestsRepository _repository;
    private readonly IValidator<SendVolunteerRequestToRevisionCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public SendVolunteerRequestToRevisionCommandHandler(
        ILogger<SendVolunteerRequestToRevisionCommandHandler> logger,
        IVolunteerRequestsRepository repository,
        IValidator<SendVolunteerRequestToRevisionCommand> validator,
        [FromKeyedServices(Modules.VolunteerRequests)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SendVolunteerRequestToRevisionCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var volunteerRequestId = VolunteerRequestId.Create(command.VolunteerRequestId);

        var volunteerRequestResult = await _repository.GetById(volunteerRequestId, cancellationToken);
        if (volunteerRequestResult.IsFailure)
            return volunteerRequestResult.Error.ToErrorList();

        var rejectionComment = RejectionComment.Create(command.RejectionComment).Value;

        var sendToRevisionResult = volunteerRequestResult.Value.SendToRevision(rejectionComment);
        if (sendToRevisionResult.IsFailure)
            return sendToRevisionResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Volunteer request {volunteerRequestId} sent to revision",
            volunteerRequestId.Value);

        return volunteerRequestId.Value;
    }
}