using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.VolunteerRequests.Application.Commands.ApproveVolunteerRequest;

public class ApproveVolunteerRequestCommandHandler : ICommandHandler<Guid, ApproveVolunteerRequestCommand>
{
    private readonly ILogger<ApproveVolunteerRequestCommand> _logger;
    private readonly IVolunteerRequestsRepository _repository;
    private readonly IAccountContract _accountContract;
    private readonly IValidator<ApproveVolunteerRequestCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveVolunteerRequestCommandHandler(
        ILogger<ApproveVolunteerRequestCommand> logger,
        IVolunteerRequestsRepository repository,
        IAccountContract accountContract,
        IValidator<ApproveVolunteerRequestCommand> validator,
        [FromKeyedServices(Modules.VolunteerRequests)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _accountContract = accountContract;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        ApproveVolunteerRequestCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        var volunteerRequestId = VolunteerRequestId.Create(command.VolunteerRequestId);

        try
        {
            var volunteerRequestResult = await _repository.GetById(volunteerRequestId, cancellationToken);

            if (volunteerRequestResult.IsFailure)
                return volunteerRequestResult.Error.ToErrorList();

            var volunteerRequest = volunteerRequestResult.Value;

            if (volunteerRequest.AdminId != command.AdminId)
                return Errors.General.ValueIsInvalid(nameof(command.AdminId)).ToErrorList();

            var volunteerExperience = volunteerRequest.VolunteerInfo.Experience;
            var requisites = volunteerRequest.VolunteerInfo.Requisites.ToList();

            var volunteerAccountResult = await _accountContract.CreateVolunteerAccount(
                volunteerRequest.UserId, volunteerExperience, requisites, cancellationToken);

            if (volunteerAccountResult.IsFailure)
                return validationResult.ToErrorsList();

            var approveResult = volunteerRequest.Approve();

            if (approveResult.IsFailure)
                return approveResult.Error.ToErrorList();

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Volunteer request {volunteerRequestId} was approved by {adminId}",
                volunteerRequestId.Value,
                command.AdminId);

            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();

            _logger.LogError(
                "An error occurred during the approval of a volunteer request {volunteerRequestId}: {errorMessage}, {stackTrace}",
                volunteerRequestId.Value,
                e.Message,
                e.StackTrace);

            return Error.Failure("approve.volunteer.request.fail", "Failed to approve volunteer request")
                .ToErrorList();
        }

        return volunteerRequestId.Value;
    }
}