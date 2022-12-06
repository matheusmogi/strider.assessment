using Strider.Posterr.Common;

namespace Strider.Posterr.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task AddAsync(TEntity entity); 
    Task<TEntity?> GetByIdAsync(Guid id); 
}