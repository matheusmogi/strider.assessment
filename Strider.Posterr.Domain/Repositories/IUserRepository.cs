using System.Text.RegularExpressions;
using Strider.Posterr.Domain.Models;

namespace Strider.Posterr.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<IEnumerable<User>> QueryByUserNameAsync(params string[] userNames);
}