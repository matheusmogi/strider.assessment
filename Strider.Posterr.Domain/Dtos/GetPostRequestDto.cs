namespace Strider.Posterr.Domain.Dtos;

public class GetPostRequestDto
{
    public Guid UserId { get; set; }
    public int PageSize { get; set; }
    public int Offset { get; set; }
}