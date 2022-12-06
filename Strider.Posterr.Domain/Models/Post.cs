
namespace Strider.Posterr.Domain.Models;

public class Post : Entity
{
    public Post()
    {
    }

    public Post(Guid id, Guid createdBy, string content, Guid? originalPost, params User[]? mentions) : base(id)
    {
        CreatedById = createdBy;
        Content = content;
        OriginalPostId = originalPost;
        UsersMentioned = mentions?.ToList();
    }

    public User CreatedBy { get; set; }
    public Guid CreatedById { get; set; }
    public string Content { get; set; }
    public Post? OriginalPost { get; set; }
    public Guid? OriginalPostId { get; set; }
    public List<User>? UsersMentioned { get; set; }
    public List<Post>? Reposted { get; set; }
}