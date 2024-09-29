using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Providers;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;
using Minio;
using PetFamily.Application.Database;
using PetFamily.Application.Files;
using PetFamily.Application.Messaging;
using PetFamily.Application.SpeciesHandlers;
using PetFamily.Application.VolunteersHandlers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Services;
using FileInfo = PetFamily.Application.Providers.FileInfo;

namespace PetFamily.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>();

        services.AddScoped<IReadDbContext, ReadDbContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHostedService<FileCleanerBackgroundService>();

        services.AddScoped<IFileCleanerService, FileCleanerService>();

        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();

        services
            .AddRepositories()
            .AddMinio(configuration);

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IVolunteerRepository, VolunteerRepository>()
            .AddScoped<ISpeciesRepository, SpeciesRepository>()
            .AddScoped<IPetRepository, PetRepository>();
    }

    private static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));

        services.AddMinio(options =>
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