using Microsoft.Azure.Cosmos.Core;
using Newtonsoft.Json;

namespace Strider.Posterr.Common;

public abstract class Entity :  IEntity
{
    [JsonProperty(PropertyName = "od")]
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