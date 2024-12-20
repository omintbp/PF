using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Database;
using PetFamily.SharedKernel;
using PetFamily.Species.Application;
using PetFamily.Species.Infrastructure.DbContexts;
using PetFamily.Species.Infrastructure.Repositories;

namespace PetFamily.Species.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesInfrastructure(this IServiceCollection services)
    {
        return services
            .AddDatabase()
            .AddDbContexts()
            .AddRepositories();
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        return services
            .AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Species);
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        return services
            .AddScoped<WriteDbContext>()
            .AddScoped<IReadDbContext, ReadDbContext>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<ISpeciesRepository, SpeciesRepository>();
    }
}