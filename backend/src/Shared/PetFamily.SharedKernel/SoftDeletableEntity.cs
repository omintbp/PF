namespace PetFamily.SharedKernel;

public abstract class SoftDeletableEntity<TId> : Entity<TId>
{
    protected SoftDeletableEntity(TId id) : base(id)
    {
    }

    public bool IsDeleted { get; protected set; }

    public DateTime DeletionDate { get; protected set; }

    public virtual void Delete()
    {
        DeletionDate = DateTime.UtcNow;
        IsDeleted = true;
    }

    public virtual void Restore()
    {
        DeletionDate = default;
        IsDeleted = false;
    }
}