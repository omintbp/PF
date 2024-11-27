using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Accounts.Presentation;
using PetFamily.Core;
using PetFamily.Species.Presentation;
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
    .AddSpeciesModule(builder.Configuration)
    .AddPetsModule(builder.Configuration)
    .AddAccountsModule(builder.Configuration)
    .AddDiscussionsModule(builder.Configuration)
    .AddVolunteerRequestsModule(builder.Configuration);

builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(VolunteersController).Assembly)
    .AddApplicationPart(typeof(AccountController).Assembly)
    .AddApplicationPart(typeof(SpeciesController).Assembly);

Inject.AddDapperTypeHandlers();

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