namespace PetFamily.Domain.Shared;

public abstract class Entity<TId> where TId : notnull
{
    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    public TId Id { get; private set; }
}