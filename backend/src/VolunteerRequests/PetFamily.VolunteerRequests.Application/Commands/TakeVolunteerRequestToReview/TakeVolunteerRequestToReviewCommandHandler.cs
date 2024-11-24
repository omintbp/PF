using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Contracts;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.VolunteerRequests.Application.Commands.TakeVolunteerRequestToReview;

public class TakeVolunteerRequestToReviewCommandHandler : ICommandHandler<Guid, TakeVolunteerRequestToReviewCommand>
{
    private readonly ILogger<TakeVolunteerRequestToReviewCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDiscussionsContract _discussionsContract;
    private readonly IValidator<TakeVolunteerRequestToReviewCommand> _validator;
    private readonly IVolunteerRequestsRepository _repository;

    public TakeVolunteerRequestToReviewCommandHandler(
        ILogger<TakeVolunteerRequestToReviewCommandHandler> logger,
        [FromKeyedServices(Modules.VolunteerRequests)]
        IUnitOfWork unitOfWork,
        IDiscussionsContract discussionsContract,
        IValidator<TakeVolunteerRequestToReviewCommand> validator,
        IVolunteerRequestsRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _discussionsContract = discussionsContract;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        TakeVolunteerRequestToReviewCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var volunteerRequestId = VolunteerRequestId.Create(command.VolunteerRequestId);
        var volunteerRequestResult = await _repository.GetById(volunteerRequestId, cancellationToken);

        if (volunteerRequestResult.IsFailure)
            return volunteerRequestResult.Error.ToErrorList();

        var volunteerRequest = volunteerRequestResult.Value;

        var createDiscussionResult = await _discussionsContract.CreateDiscussionHandler(
            volunteerRequest.UserId,
            command.AdminId,
            volunteerRequestId.Value,
            cancellationToken);

        if (createDiscussionResult.IsFailure)
            return createDiscussionResult.Error.ToErrorList();

        var discussionId = createDiscussionResult.Value;

        var takeToReviewResult = volunteerRequest.TakeToReview(command.AdminId, discussionId);

        if (takeToReviewResult.IsFailure)
            return takeToReviewResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        return volunteerRequest.Id.Value;
    }
}