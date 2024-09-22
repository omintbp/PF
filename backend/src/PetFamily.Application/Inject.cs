using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Options;
using PetFamily.Application.Volunteers.Commands.AddPet;
using PetFamily.Application.Volunteers.Commands.AddPetPhotos;
using PetFamily.Application.Volunteers.Commands.Create;
using PetFamily.Application.Volunteers.Commands.Delete;
using PetFamily.Application.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Application.Volunteers.Commands.UpdateRequisites;
using PetFamily.Application.Volunteers.Commands.UpdateSocialNetworks;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateSocialNetworksHandler>();
        services.AddScoped<UpdateRequisitesHandler>();
        services.AddScoped<DeleteVolunteerRequestHandler>();
        services.AddScoped<AddPetCommandHandler>();
        services.AddScoped<AddPetPhotosCommandHandler>();

        services.Configure<ImageUploadOptions>(configuration.GetSection(ImageUploadOptions.IMAGE_UPLAOD_OPTIONS));

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
}