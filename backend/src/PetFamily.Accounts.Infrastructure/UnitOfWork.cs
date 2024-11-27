using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Accounts.Infrastructure.DbContexts.Write;
using PetFamily.Core.Database;

namespace PetFamily.Accounts.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AccountWriteDbContext _context;

    public UnitOfWork(AccountWriteDbContext context)
    {
        _context = context;
    }
    
    public async Task<DbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}