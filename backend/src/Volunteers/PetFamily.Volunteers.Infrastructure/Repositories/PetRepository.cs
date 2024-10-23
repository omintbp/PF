using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.Volunteers.Infrastructure.Repositories;

public class PetRepository : IPetRepository
{
    private readonly WriteDbContext _context;

    public PetRepository(WriteDbContext context)
    {
        _context = context;
    }

    public void Delete(Pet pet)
    {
        _context.Remove(pet);
    }
}