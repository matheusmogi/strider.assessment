using Strider.Posterr.Domain.Models;

namespace Strider.Posterr.Domain.Dtos;

public class PostResponseDto
{
    public PostResponseDto(IEnumerable<Post> entities, int totalRecords)
    {
        TotalRecords = totalRecords;
        Posts = entities.Select(a => new PostDto(a)).ToArray();
    }

    public IEnumerable<PostDto> Posts { get; set; }
    public int TotalRecords { get; set; }
}