using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.VolunteerRequests.Domain.AggregateRoot;
using PetFamily.VolunteerRequests.Domain.ValueObjects;

namespace PetFamily.VolunteerRequests.Application.Commands.RejectVolunteerRequest;

public class RejectVolunteerRequestCommandHandler : ICommandHandler<Guid, RejectVolunteerRequestCommand>
{
    private readonly ILogger<RejectVolunteerRequestCommandHandler> _logger;
    private readonly IVolunteerRequestsRepository _repository;
    private readonly IVolunteerRequestBanRepository _banRepository;
    private readonly IValidator<RejectVolunteerRequestCommand> _validator;
    private readonly VolunteerRequestsOptions _options;
    private readonly IUnitOfWork _unitOfWork;

    public RejectVolunteerRequestCommandHandler(
        ILogger<RejectVolunteerRequestCommandHandler> logger,
        IVolunteerRequestsRepository repository,
        IVolunteerRequestBanRepository banRepository,
        IValidator<RejectVolunteerRequestCommand> validator,
        IOptions<VolunteerRequestsOptions> options,
        [FromKeyedServices(Modules.VolunteerRequests)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _banRepository = banRepository;
        _validator = validator;
        _options = options.Value;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        RejectVolunteerRequestCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var volunteerRequestId = VolunteerRequestId.Create(command.VolunteerRequestId);

        await using var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var volunteerRequestResult = await _repository.GetById(volunteerRequestId, cancellationToken);
            if (volunteerRequestResult.IsFailure)
                return volunteerRequestResult.Error.ToErrorList();

            var volunteerRequest = volunteerRequestResult.Value;

            if (volunteerRequest.AdminId != command.AdminId)
                return Errors.General.ValueIsInvalid(nameof(command.AdminId)).ToErrorList();

            var rejectionComment = RejectionComment.Create(command.RejectionComment).Value;

            var rejectionResult = volunteerRequest.Reject(rejectionComment);
            if (rejectionResult.IsFailure)
                return rejectionResult.Error.ToErrorList();

            var userToBan = volunteerRequest.UserId;
            var banStartDate = DateTime.UtcNow;
            var banEndDate = banStartDate.AddDays(_options.BanInDays);
            var volunteerRequestBanId = VolunteerRequestBanId.NewVolunteerRequestBanId();

            var volunteerRequestBanResult = VolunteerRequestBan.Create(
                volunteerRequestBanId,
                userToBan,
                banStartDate,
                banEndDate);

            if (volunteerRequestBanResult.IsFailure)
                return volunteerRequestBanResult.Error.ToErrorList();

            await _banRepository.Add(volunteerRequestBanResult.Value, cancellationToken);

            await _unitOfWork.SaveChanges(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("Rejected volunteer request {volunteerRequestId}", volunteerRequestId);

            return volunteerRequestId.Value;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError(
                "Error during the rejection of volunteer request {volunteerRequestId}: {message}, {stackTrace}",
                volunteerRequestId.Value,
                e.Message,
                e.StackTrace);

            return Error.Failure( 
                "reject.volunteer.request.fail",
                "Error during the rejection of volunteer request").ToErrorList();
        }
    }
}