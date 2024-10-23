namespace PetFamily.SharedKernel;

public interface ISoftDeletable
{
    void Delete();

    void Restore();
}