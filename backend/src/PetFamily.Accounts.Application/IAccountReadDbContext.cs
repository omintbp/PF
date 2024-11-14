using PetFamily.Core.DTOs.Accounts;

namespace PetFamily.Accounts.Application;

public interface IAccountReadDbContext
{
    IQueryable<UserDto> Users { get; }
}