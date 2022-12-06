using Strider.Posterr.Common;
using Strider.Posterr.Domain.Repositories;

namespace Strider.Posterr.RelationalData.Repositories;

public class EntityFrameworkUnitOfWork : IUnitOfWork
{
    private readonly PosterrSqlDbContext _context;

    public EntityFrameworkUnitOfWork(PosterrSqlDbContext context)
    {
        Check.ArgumentNotNull(context);

        _context = context;
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}