namespace PetFamily.Volunteers.Application;

public interface IDeletedVolunteersCleanerService
{
    Task Process(CancellationToken cancellationToken);
}