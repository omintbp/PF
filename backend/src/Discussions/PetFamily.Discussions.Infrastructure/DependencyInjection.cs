using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Database;
using PetFamily.Discussions.Application;
using PetFamily.Discussions.Infrastructure.DbContexts;
using PetFamily.Discussions.Infrastructure.Repositories;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDiscussionsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddDatabase()
            .AddRepositories()
            .AddDbContexts();
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        return services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Discussions);
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        return services.AddScoped<WriteDbContext>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<IDiscussionsRepository, DiscussionsRepository>();
    }
}