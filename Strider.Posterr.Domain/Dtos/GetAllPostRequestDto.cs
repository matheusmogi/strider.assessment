namespace Strider.Posterr.Domain.Dtos;

public class GetAllPostRequestDto
{
    public Guid? UserId { get; set; }
    public int PageSize { get; set; }
    public int Offset { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}