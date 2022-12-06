namespace Strider.Posterr.Domain.Dtos;

public class PostRequestDto
{
    public PostRequestDto(string content, Guid? originalPostId)
    {
        Id = new Guid();
        Content = content;
        OriginalPostId = originalPostId;
    }

    public Guid Id { get; set; }
    public Guid CreatedById { get; set; }
    public string Content { get; set; }
    public Guid? OriginalPostId { get; set; }
}