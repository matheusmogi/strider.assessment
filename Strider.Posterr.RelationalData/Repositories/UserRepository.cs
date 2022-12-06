using Microsoft.EntityFrameworkCore;
using Strider.Posterr.Domain.Models;
using Strider.Posterr.Domain.Repositories;

namespace Strider.Posterr.RelationalData.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(PosterrSqlDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<User>> QueryByUserNameAsync(params string[] userNames)
    {
        return await Set.Where(u => userNames.Contains(u.UserName))
            .ToListAsync();
    }
}