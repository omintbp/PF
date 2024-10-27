using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services
            .AddCommandHandlers()
            .AddQueryHandlers();
    }
    
    private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.Scan(type => type.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes =>
                classes.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        services.Scan(type => type.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes =>
                classes.AssignableToAny(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }
}