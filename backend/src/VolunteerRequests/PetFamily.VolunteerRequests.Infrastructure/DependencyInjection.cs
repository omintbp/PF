using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Database;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Application;
using PetFamily.VolunteerRequests.Infrastructure.DbContexts;
using PetFamily.VolunteerRequests.Infrastructure.Repositories;

namespace PetFamily.VolunteerRequests.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerRequestsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddOptions(configuration)
            .AddDatabase()
            .AddRepositories()
            .AddDbContexts();
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<VolunteerRequestsOptions>(
            configuration.GetSection(VolunteerRequestsOptions.VOLUNTEER_REQUESTS));
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        return services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.VolunteerRequests);
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        return services.AddScoped<WriteDbContext>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IVolunteerRequestsRepository, VolunteerRequestsRepository>()
            .AddScoped<IVolunteerRequestBanRepository, VolunteerRequestBanRepository>();
    }
}