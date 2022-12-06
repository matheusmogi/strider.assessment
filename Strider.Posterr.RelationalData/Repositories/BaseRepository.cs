using Microsoft.EntityFrameworkCore;
using Strider.Posterr.Common;
using Strider.Posterr.Domain.Repositories;

namespace Strider.Posterr.RelationalData.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected DbSet<TEntity> Set { get; }

    protected BaseRepository(PosterrSqlDbContext context)
    {
        Set = context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity)
    {
        Check.ArgumentNotNull(entity);
        await Set.AddAsync(entity);
    }
     
    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await Set.FindAsync(id);
    }
 
}