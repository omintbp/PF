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

        await using var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        var volunteerRequestId = VolunteerRequestId.Create(command.VolunteerRequestId);

        try
        {
            var volunteerRequestResult = await _repository.GetById(volunteerRequestId, cancellationToken);
            if (volunteerRequestResult.IsFailure)
                return volunteerRequestResult.Error.ToErrorList();

            var rejectionComment = RejectionComment.Create(command.RejectionComment).Value;

            var sendToRevisionResult = volunteerRequestResult.Value.SendToRevision(rejectionComment);
            if (sendToRevisionResult.IsFailure)
                return sendToRevisionResult.Error.ToErrorList();

            await _unitOfWork.SaveChanges(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("Volunteer request {volunteerRequestId} sent to revision",
                volunteerRequestId.Value);

            return volunteerRequestId.Value;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError(
                "Error while sending volunteer request {volunteerRequestId} to revision : {message}, {stackTrace}",
                volunteerRequestId.Value,
                e.Message,
                e.StackTrace);

            return Error.Failure(
                "revision.volunteer.request.fail",
                "Error while sending volunteer request to revision").ToErrorList();
        }
    }
}