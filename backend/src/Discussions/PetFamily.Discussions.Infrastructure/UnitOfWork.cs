using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Core.Database;
using PetFamily.Discussions.Infrastructure.DbContexts;

namespace PetFamily.Discussions.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteDbContext _context;

    public UnitOfWork(WriteDbContext context)
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