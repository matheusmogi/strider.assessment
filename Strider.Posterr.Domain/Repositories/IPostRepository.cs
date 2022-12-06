using Strider.Posterr.Domain.Dtos;
using Strider.Posterr.Domain.Models;

namespace Strider.Posterr.Domain.Repositories;

public interface IPostRepository : IRepository<Post>
{ 
    Task<QueryResult<Post>> GetPostsByUserAsync(Guid userId, int pageSize, int offset);
    Task<QueryResult<Post>> GetAllPostsAsync(GetAllPostRequestDto requestDto);
    int GetTodayPostsByUser(Guid userId);
}