using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Core.Database;

namespace PetFamily.Accounts.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AuthorizationDbContext _context;

    public UnitOfWork(AuthorizationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}