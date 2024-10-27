using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts;

namespace PetFamily.Accounts.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountPresentation(this IServiceCollection services)
    {
        services.AddScoped<IAccountContract, AccountContract>();
        return services;
    }
}