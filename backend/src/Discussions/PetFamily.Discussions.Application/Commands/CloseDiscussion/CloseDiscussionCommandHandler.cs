using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.Discussions.Application.Commands.CloseDiscussion;

public class CloseDiscussionCommandHandler : ICommandHandler<Guid, CloseDiscussionCommand>
{
    private readonly ILogger<CloseDiscussionCommandHandler> _logger;
    private readonly IDiscussionsRepository _repository;
    private readonly IValidator<CloseDiscussionCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public CloseDiscussionCommandHandler(
        ILogger<CloseDiscussionCommandHandler> logger,
        IDiscussionsRepository repository,
        IValidator<CloseDiscussionCommand> validator,
        [FromKeyedServices(Modules.Discussions)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CloseDiscussionCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        var discussionId = DiscussionId.Create(command.DiscussionId);

        try
        {
            var discussionResult = await _repository.GetById(discussionId, cancellationToken);
            if (discussionResult.IsFailure)
                return discussionResult.Error.ToErrorList();

            var discussion = discussionResult.Value;

            var closeResult = discussion.Close(command.UserId);
            if (closeResult.IsFailure)
                return closeResult.Error.ToErrorList();

            await _unitOfWork.SaveChanges(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("Discussion {discussionId} closed", discussion.Id.Value);

            return discussion.Id.Value;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError("Error while closing of discussion {discussionId}: {message}, {stackTrace}",
                discussionId.Value,
                e.Message,
                e.StackTrace);

            return Error.Failure("discussion.close.fail", "Failed to close discussion")
                .ToErrorList();
        }
    }
}