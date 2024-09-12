using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Files.Delete;
using PetFamily.Application.Files.Get;
using PetFamily.Application.Files.Upload;
using PetFamily.Application.Options;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.AddPetPhotos;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;

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

        services.AddFileHandlers();

        services.Configure<ImageUploadOptions>(configuration.GetSection(ImageUploadOptions.IMAGE_UPLAOD_OPTIONS));
        
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }

    private static IServiceCollection AddFileHandlers(this IServiceCollection services)
    {
        services.AddScoped<UploadFileRequestHandler>();
        services.AddScoped<DeleteFileRequestHandler>();
        services.AddScoped<GetFileRequestHandler>();

        return services;
    }
}