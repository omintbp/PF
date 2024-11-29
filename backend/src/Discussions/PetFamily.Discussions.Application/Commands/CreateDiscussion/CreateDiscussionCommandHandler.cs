using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Domain.AggregateRoot;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Commands.CreateDiscussion;

public class CreateDiscussionCommandHandler : ICommandHandler<Guid, CreateDiscussionCommand>
{
    private readonly ILogger<CreateDiscussionCommandHandler> _logger;
    private readonly IDiscussionsRepository _repository;
    private readonly IValidator<CreateDiscussionCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDiscussionCommandHandler(
        ILogger<CreateDiscussionCommandHandler> logger,
        IDiscussionsRepository repository,
        IValidator<CreateDiscussionCommand> validator,
        [FromKeyedServices(Modules.Discussions)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateDiscussionCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var discussionsResult = await _repository.GetByRelationId(command.RelationId, cancellationToken);
            if (discussionsResult.Any())
                return Errors.General.AlreadyExist("Discussion").ToErrorList();
            
            var discussionResult = Discussion.Create(command.RelationId, command.Users.ToList());

            if (discussionResult.IsFailure)
                return discussionResult.Error.ToErrorList();

            var discussion = discussionResult.Value;

            await _repository.Add(discussion, cancellationToken);

            await _unitOfWork.SaveChanges(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("Discussion {discussionId} created", discussion.Id.Value);

            return discussion.Id.Value;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError("Error while creation of discussion: {message}, {stackTrace}",
                e.Message,
                e.StackTrace);

            return Error.Failure("discussion.create.fail", "Failed to create discussion")
                .ToErrorList();
        }
    }
}