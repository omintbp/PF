using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Core.Database;
using PetFamily.Core.Messaging;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Application.Files;
using PetFamily.Volunteers.Application.Providers;
using PetFamily.Volunteers.Infrastructure.BackgroundServices;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using PetFamily.Volunteers.Infrastructure.MessageQueues;
using PetFamily.Volunteers.Infrastructure.Options;
using PetFamily.Volunteers.Infrastructure.Providers;
using PetFamily.Volunteers.Infrastructure.Repositories;
using PetFamily.Volunteers.Infrastructure.Services;
using FileInfo = PetFamily.Volunteers.Application.Providers.FileInfo;
using ServiceCollectionExtensions = Minio.ServiceCollectionExtensions;

namespace PetFamily.Volunteers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteersInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();

        services
            .AddEntitiesCleaner(configuration)
            .AddDbContexts()
            .AddDatabase()
            .AddRepositories()
            .AddFiles()
            .AddMinio(configuration);

        return services;
    }

    private static IServiceCollection AddEntitiesCleaner(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddHostedService<EntitiesCleanerBackgroundService>()
            .Configure<EntitiesCleanerOptions>(configuration.GetSection(EntitiesCleanerOptions.ENTITIES_CLEANER))
            .AddScoped<IDeletedVolunteersCleanerService, DeletedVolunteersCleanerService>()
            .AddScoped<IDeletedPetsCleanerService, DeletedPetsCleanerService>();
    }

    private static IServiceCollection AddFiles(this IServiceCollection services)
    {
        return services
            .AddHostedService<FileCleanerBackgroundService>()
            .AddScoped<IFileCleanerService, FileCleanerService>();
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        return services
            .AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Pets)
            .AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
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
            .AddScoped<IVolunteerRepository, VolunteerRepository>()
            .AddScoped<IPetRepository, PetRepository>();
    }

    private static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));

        ServiceCollectionExtensions.AddMinio(services, options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");

            options.WithEndpoint(minioOptions.Endpoint);

            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();

        return services;
    }
}