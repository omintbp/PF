using PetFamily.Accounts.Infrastructure;
using PetFamily.Accounts.Presentation;
using PetFamily.Species.Application;
using PetFamily.Species.Infrastructure;
using PetFamily.Species.Presentation;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Infrastructure;
using PetFamily.Volunteers.Presentation;
using PetFamily.Volunteers.Presentation.Volunteers;
using PetFamily.WEB;
using PetFamily.WEB.Extensions;
using PetFamily.WEB.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();

builder.Services.AddLogger(builder.Configuration);

builder.Services
    .AddAccountApplication()
    .AddAccountInfrastructure(builder.Configuration);

builder.Services
    .AddSpeciesInfrastructure()
    .AddSpeciesApplication()
    .AddSpeciesPresentation();

builder.Services
    .AddVolunteersInfrastructure(builder.Configuration)
    .AddVolunteersApplication(builder.Configuration)
    .AddVolunteersPresentation();

builder.Services.AddControllers()
    .AddApplicationPart(typeof(VolunteersController).Assembly)
    .AddApplicationPart(typeof(AccountController).Assembly)
    .AddApplicationPart(typeof(SpeciesController).Assembly);

var app = builder.Build();

app.UseExceptionHandleMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();