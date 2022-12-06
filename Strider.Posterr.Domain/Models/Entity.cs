namespace Strider.Posterr.Domain.Models;

public abstract class Entity :  IEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }

    protected Entity()
    {
        CreatedOn = DateTime.UtcNow;
    }
    protected Entity(Guid id)
    {
        Id = id;
        CreatedOn = DateTime.UtcNow;
    }
}