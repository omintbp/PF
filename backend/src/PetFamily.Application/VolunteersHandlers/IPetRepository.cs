using PetFamily.Domain.PetManagement.Entities;

namespace PetFamily.Application.VolunteersHandlers;

public interface IPetRepository
{
    void Delete(Pet pet);
}