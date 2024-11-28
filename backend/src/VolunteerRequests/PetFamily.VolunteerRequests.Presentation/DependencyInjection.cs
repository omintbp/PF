using Microsoft.Extensions.DependencyInjection;
using PetFamily.VolunteerRequests.Contracts;

namespace PetFamily.VolunteerRequests.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerRequestsPresentation(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRequestsContract, VolunteerRequestsContract>();
        return services;
    }
}