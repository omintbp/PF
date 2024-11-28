using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Models;
using PetFamily.Framework.Authorization;

namespace PetFamily.Accounts.Infrastructure.Authorization;

public class PermissionAuthorizationHandler(IServiceScopeFactory factory)
    : AuthorizationHandler<PermissionAttribute>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute requirement)
    {
        await using var scope = factory.CreateAsyncScope();
        var accountContract = scope.ServiceProvider.GetRequiredService<IAccountContract>();

        var idClaim = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.Id);

        var parseResult = Guid.TryParse(idClaim?.Value, out var userId);
        if (parseResult == false)
        {
            context.Fail();
            return;
        }

        var isUserHasPermission = await accountContract.CheckIfUserHasPermission(userId, requirement.Code);

        if (isUserHasPermission)
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}