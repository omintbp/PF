using Microsoft.OpenApi.Models;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Accounts.Presentation;
using PetFamily.Discussions.Infrastructure;
using PetFamily.Discussions.Presentation;
using PetFamily.Species.Application;
using PetFamily.Species.Infrastructure;
using PetFamily.Species.Presentation;
using PetFamily.VolunteerRequests.Application;
using PetFamily.VolunteerRequests.Infrastructure;
using PetFamily.VolunteerRequests.Presentation;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Infrastructure;
using PetFamily.Volunteers.Presentation;

namespace PetFamily.WEB;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddSpeciesModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddSpeciesInfrastructure()
            .AddSpeciesApplication()
            .AddSpeciesPresentation();
    }

    public static IServiceCollection AddPetsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddVolunteersInfrastructure(configuration)
            .AddVolunteersApplication(configuration)
            .AddVolunteersPresentation();
    }

    public static IServiceCollection AddAccountsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddAccountApplication()
            .AddAccountInfrastructure(configuration)
            .AddAccountPresentation();
    }

    public static IServiceCollection AddDiscussionsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddDiscussionsInfrastructure(configuration)
            .AddDiscussionsPresentation();
    }

    public static IServiceCollection AddVolunteerRequestsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddVolunteerRequestsInfrastructure(configuration)
            .AddVolunteerRequestsApplication(configuration)
            .AddVolunteerRequestsPresentation();
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "My API",
                Version = "v1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });

        return services;
    }
}