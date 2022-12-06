using Microsoft.EntityFrameworkCore;
using Strider.Posterr.Domain.Models;
using Strider.Posterr.Domain.Repositories;

namespace Strider.Posterr.RelationalData.Extensions;

public static class PaginationExtensions
{
    public static async Task<QueryResult<T>> PaginateAsync<T>(this IQueryable<T> query, int pageSize, int offset) where T : Entity
    {
        var results = query.OrderByDescending(a => a.CreatedOn).Skip(offset).Take(pageSize);
        return new QueryResult<T>(await results.ToListAsync(), await query.CountAsync());
    }
}