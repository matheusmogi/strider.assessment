using Microsoft.EntityFrameworkCore;
using Strider.Posterr.Domain.Dtos;
using Strider.Posterr.Domain.Factory;
using Strider.Posterr.Domain.Models;
using Strider.Posterr.Domain.Repositories;
using Strider.Posterr.RelationalData.Extensions;

namespace Strider.Posterr.RelationalData.Repositories;

public class PostRepository : BaseRepository<Post>, IPostRepository
{
    public PostRepository(PosterrSqlDbContext context) : base(context)
    {
    }

    public async Task<QueryResult<Post>> GetPostsByUserAsync(Guid userId, int pageSize, int offset)
    {
        return await Set
            .Include(s => s.CreatedBy)
            .Include(s => s.OriginalPost)
            .Include(s => s.UsersMentioned)
            .Where(s =>
                s.CreatedById == userId ||
                (s.OriginalPost != null && s.OriginalPost.CreatedById == userId) ||
                s.UsersMentioned!.AsEnumerable().Any(a => a.Id == userId))
            .PaginateAsync(pageSize, offset);
    }

    public async Task<QueryResult<Post>> GetAllPostsAsync(GetAllPostRequestDto requestDto)
    {
        return await Set
            .Include(s => s.CreatedBy)
            .Include(s => s.OriginalPost)
            .Include(s => s.UsersMentioned)
            .Where(s =>
                (!requestDto.UserId.HasValue || s.CreatedById == requestDto.UserId ||
                 (s.OriginalPost != null && s.OriginalPost.CreatedById == requestDto.UserId) ||
                 s.UsersMentioned!.AsEnumerable().Any(a => a.Id == requestDto.UserId))
                && (!requestDto.From.HasValue || s.CreatedOn.Date >= requestDto.From.Value.Date)
                && (!requestDto.To.HasValue || s.CreatedOn.Date <= requestDto.To.Value.Date))
            .PaginateAsync(requestDto.PageSize, requestDto.Offset);
    }

    public int GetTodayPostsByUser(Guid userId)
    {
        return Set.Count(a => a.CreatedById == userId && a.CreatedOn.Date == DateTime.Today);
    }
}