using System.Security.Claims;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Managers;
using PetFamily.Accounts.Contracts.Response;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
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

        var claimsResult = await _tokenProvider.GetClaims(command.AccessToken, cancellationToken);
        if (claimsResult.IsFailure)
            return claimsResult.Error.ToErrorList();

        var claims = claimsResult.Value;

        var userIdString = claims.FirstOrDefault(x => x.Type == CustomClaims.Id)?.Value;
        if (Guid.TryParse(userIdString, out var userId) == false)
        {
            return Errors.User.TokenInvalid().ToErrorList();
        }

        if (refreshSession.UserId != userId)
        {
            return Errors.User.TokenInvalid().ToErrorList();
        }

        var jtiString = claims.FirstOrDefault(x => x.Type == CustomClaims.Jti)?.Value;
        if (Guid.TryParse(jtiString, out var jti) == false)
        {
            return Errors.User.TokenInvalid().ToErrorList();
        }

        if (refreshSession.Jti != jti)
        {
            return Errors.User.TokenInvalid().ToErrorList();
        }

        _refreshSessionManager.Delete(refreshSession);
        await _unitOfWork.SaveChanges(cancellationToken);

        var accessToken = _tokenProvider.GenerateAccessToken(refreshSession.User);

        var refreshToken = await _tokenProvider.GenerateRefreshToken(
            refreshSession.User,
            accessToken.Jti,
            cancellationToken);

        return new LoginResponse(accessToken.Token, refreshToken);
    }
}