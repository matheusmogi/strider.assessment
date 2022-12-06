 

namespace Strider.Posterr.Domain.Repositories;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task SaveChangesAsync();
}