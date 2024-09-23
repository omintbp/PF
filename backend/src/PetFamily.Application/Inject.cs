using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Options;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddCommandHandlers()
            .AddQueryHandlers()
            .AddValidatorsFromAssembly(typeof(Inject).Assembly)
            .Configure<ImageUploadOptions>(configuration.GetSection(ImageUploadOptions.IMAGE_UPLAOD_OPTIONS));

        return services;
    }

    private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.Scan(type => type.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes =>
                classes.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        services.Scan(type => type.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes =>
                classes.AssignableToAny(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }
}