using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.Discussions.Application.Commands.DeleteMessage;

public class DeleteMessageCommandHandler : ICommandHandler<Guid, DeleteMessageCommand>
{
    private readonly ILogger<DeleteMessageCommandHandler> _logger;
    private readonly IDiscussionsRepository _repository;
    private readonly IValidator<DeleteMessageCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMessageCommandHandler(
        ILogger<DeleteMessageCommandHandler> logger,
        IDiscussionsRepository repository,
        IValidator<DeleteMessageCommand> validator,
        [FromKeyedServices(Modules.Discussions)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteMessageCommand command,
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
            
            if (discussion.IsActive == false)
                return Errors.Discussion.DiscussionNotActive(discussion.Id.Value).ToErrorList();

            var messageId = MessageId.Create(command.MessageId);

            var deleteResult = discussion.DeleteMessage(command.UserId, messageId);

            if (deleteResult.IsFailure)
                return deleteResult.Error.ToErrorList();

            await _unitOfWork.SaveChanges(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("Message {messageId} deleted in {discussionId}",
                messageId.Value,
                discussion.Id.Value);

            return discussion.Id.Value;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError("Error while deleting message {messageId} in  {discussionId}: {message}, {stackTrace}",
                command.MessageId,
                command.DiscussionId,
                e.Message,
                e.StackTrace);

            return Error.Failure("discussion.delete.message.fail", "Failed to delete message in discussion")
                .ToErrorList();
        }
    }
}