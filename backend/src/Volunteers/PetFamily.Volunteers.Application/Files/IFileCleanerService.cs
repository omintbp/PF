namespace PetFamily.Volunteers.Application.Files;

public interface IFileCleanerService
{
    Task Process(CancellationToken cancellationToken);
}