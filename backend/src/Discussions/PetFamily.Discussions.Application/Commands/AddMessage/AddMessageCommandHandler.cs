using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Domain.Entities;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.Discussions.Application.Commands.AddMessage;

public class AddMessageCommandHandler : ICommandHandler<Guid, AddMessageCommand>
{
    private readonly ILogger<AddMessageCommandHandler> _logger;
    private readonly IDiscussionsRepository _repository;
    private readonly IValidator<AddMessageCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AddMessageCommandHandler(
        ILogger<AddMessageCommandHandler> logger,
        IDiscussionsRepository repository,
        IValidator<AddMessageCommand> validator,
        [FromKeyedServices(Modules.Discussions)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddMessageCommand command,
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

            var messageText = Text.Create(command.Message).Value;
            var message = Message.Create(command.UserId, messageText).Value;

            var addMessageResult = discussion.AddMessage(command.UserId, message);
            if (addMessageResult.IsFailure)
                return addMessageResult.Error.ToErrorList();

            await _unitOfWork.SaveChanges(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("Message {messageId} added to discussion {discussionId}",
                message.Id.Value,
                discussion.Id.Value);

            return message.Id.Value;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError("Error while adding message to {discussionId}: {message}, {stackTrace}",
                discussionId.Value,
                e.Message,
                e.StackTrace);

            return Error.Failure("discussion.add.message.fail", "Failed to add message to discussion")
                .ToErrorList();
        }
    }
}