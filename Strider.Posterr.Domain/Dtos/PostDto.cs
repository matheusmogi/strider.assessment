using Strider.Posterr.Domain.Models;

namespace Strider.Posterr.Domain.Dtos;

public class PostDto
{
    public PostDto(Post post)
    {
        if (post == null)
            return;
        Id = post.Id;
        Content = post.Content;
        CreatedById = post.CreatedById;
        CreatedBy = new UserDto(post.CreatedBy);
        
        if (post.OriginalPost != null)
        {
            OriginalPostId = post.OriginalPost.Id;
            OriginalPost = new PostDto(post.OriginalPost);
        }
        if (post.UsersMentioned?.Count > 0)
        {
            UsersMentioned = post.UsersMentioned.Select(a => new UserDto(a)).ToList();
        }
    }

    public Guid Id { get; }
    public UserDto CreatedBy { get; }
    public Guid CreatedById { get; }
    public string Content { get; }
    public PostDto? OriginalPost { get; }
    public Guid? OriginalPostId { get; }
    public List<UserDto>? UsersMentioned { get; }
}