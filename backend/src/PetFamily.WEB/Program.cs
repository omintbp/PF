using PetFamily.Accounts.Application;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Accounts.Presentation;
using PetFamily.Discussions.Infrastructure;
using PetFamily.Discussions.Presentation;
using PetFamily.Species.Application;
using PetFamily.Species.Infrastructure;
using PetFamily.Species.Presentation;
using PetFamily.VolunteerRequests.Infrastructure;
using PetFamily.VolunteerRequests.Presentation;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Infrastructure;
using PetFamily.Volunteers.Presentation;
using PetFamily.Volunteers.Presentation.Volunteers;
using PetFamily.WEB;
using PetFamily.WEB.Extensions;
using PetFamily.WEB.Middlewares;
using Serilog;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();

builder.Services.AddLogger(builder.Configuration);

builder.Services
    .AddAccountApplication()
    .AddAccountInfrastructure(builder.Configuration)
    .AddAccountPresentation();

builder.Services
    .AddVolunteerRequestsInfrastructure(builder.Configuration)
    .AddVolunteerRequestsPresentation();

builder.Services
    .AddDiscussionsInfrastructure(builder.Configuration)
    .AddDiscussionsPresentation();

builder.Services
    .AddSpeciesInfrastructure()
    .AddSpeciesApplication()
    .AddSpeciesPresentation();

builder.Services
    .AddVolunteersInfrastructure(builder.Configuration)
    .AddVolunteersApplication(builder.Configuration)
    .AddVolunteersPresentation();

builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(VolunteersController).Assembly)
    .AddApplicationPart(typeof(AccountController).Assembly)
    .AddApplicationPart(typeof(SpeciesController).Assembly);

var app = builder.Build();

var accountSeedingService = app.Services.GetRequiredService<AccountsSeeder>();
await accountSeedingService.Seed();

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