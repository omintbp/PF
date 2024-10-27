using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeeder
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AccountsSeeder(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Seed()
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();

        var service = scope.ServiceProvider.GetRequiredService<AccountsSeedingService>();

        await service.Seed();
    }
}