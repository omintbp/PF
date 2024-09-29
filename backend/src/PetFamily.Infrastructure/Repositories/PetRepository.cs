using PetFamily.Application.VolunteersHandlers;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

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