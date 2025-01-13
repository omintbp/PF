using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Managers;
using PetFamily.Accounts.Contracts.Response;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<LoginResponse, RefreshTokenCommand>
{
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IValidator<RefreshTokenCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommandHandler(
        IRefreshSessionManager refreshSessionManager,
        ITokenProvider tokenProvider,
        IValidator<RefreshTokenCommand> validator,
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork)
    {
        _refreshSessionManager = refreshSessionManager;
        _tokenProvider = tokenProvider;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginResponse, ErrorList>> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorsList();

        var refreshSessionResult = await _refreshSessionManager.GetByRefreshToken(
            command.RefreshToken,
            cancellationToken);

        if (refreshSessionResult.IsFailure)
            return refreshSessionResult.Error.ToErrorList();

        var refreshSession = refreshSessionResult.Value;

        if (refreshSession.ExpiresIn < DateTime.UtcNow)
            return Errors.User.RefreshTokenExpired().ToErrorList();

        _refreshSessionManager.Delete(refreshSession);
        await _unitOfWork.SaveChanges(cancellationToken);

        var accessToken = _tokenProvider.GenerateAccessToken(refreshSession.User);

        var refreshToken = await _tokenProvider.GenerateRefreshToken(
            refreshSession.User,
            accessToken.Jti,
            cancellationToken);

        var user = refreshSession.User;

        var userRoles = user.Roles
            .Where(r => string.IsNullOrEmpty(r.Name) == false)
            .Select(r => r.Name!.ToLower());

        return new LoginResponse(accessToken.Token, refreshToken, user.Id, userRoles);
    }
}