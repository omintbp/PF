using PetFamily.Volunteers.Domain.Entities;

namespace PetFamily.Volunteers.Application;

public interface IPetRepository
{
    void Delete(Pet pet);
}