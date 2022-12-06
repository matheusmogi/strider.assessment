using Strider.Posterr.Common;

namespace Strider.Posterr.Domain.Models;

public class User :  Entity
{
    public string UserName { get; set; }
    public IEnumerable<Post>? PostsCreated { get; set; }
    public IEnumerable<Post>? PostsMentioned { get; set; }
}