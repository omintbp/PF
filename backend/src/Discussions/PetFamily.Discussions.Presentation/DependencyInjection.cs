using Microsoft.Extensions.DependencyInjection;
using PetFamily.Discussions.Application;
using PetFamily.Discussions.Contracts;

namespace PetFamily.Discussions.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDiscussionsPresentation(this IServiceCollection services)
    {
        services.AddScoped<IDiscussionsContract, DiscussionsContract>();
        return services;
    }
}