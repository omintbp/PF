using Microsoft.AspNetCore.Identity;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Extensions;

public static class IdentityExtensions
{
    public static ErrorList ToErrorList(this IEnumerable<IdentityError> identityErrors)
    {
        var domainErrors = identityErrors.Select(e => 
            Error.Failure(e.Code, e.Description));

        return new ErrorList(domainErrors);
    }
}