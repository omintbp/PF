using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.Discussions.Application.Commands.UpdateMessage;

public class UpdateMessageCommandHandler : ICommandHandler<Guid, UpdateMessageCommand>
{
    private readonly ILogger<UpdateMessageCommandHandler> _logger;
    private readonly IDiscussionsRepository _repository;
    private readonly IValidator<UpdateMessageCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMessageCommandHandler(
        ILogger<UpdateMessageCommandHandler> logger,
        IDiscussionsRepository repository,
        IValidator<UpdateMessageCommand> validator,
        [FromKeyedServices(Modules.Discussions)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMessageCommand command,
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
            var messageText = Text.Create(command.Message).Value;

            var updateMessageResult = discussion.UpdateMessage(
                messageId,
                command.UserId,
                messageText);

            if (updateMessageResult.IsFailure)
                return updateMessageResult.Error.ToErrorList();

            await _unitOfWork.SaveChanges(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("Message {messageId} updated in {discussionId}",
                messageId.Value,
                discussion.Id.Value);

            return messageId.Value;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError("Error while updating message {messageId} in  {discussionId}: {message}, {stackTrace}",
                command.MessageId,
                command.DiscussionId,
                e.Message,
                e.StackTrace);

            return Error.Failure("discussion.update.message.fail", "Failed to update message in discussion")
                .ToErrorList();
        }
    }
}