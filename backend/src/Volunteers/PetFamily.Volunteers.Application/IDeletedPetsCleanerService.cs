namespace PetFamily.Volunteers.Application;

public interface IDeletedPetsCleanerService
{
    Task Process(CancellationToken cancellationToken);
}