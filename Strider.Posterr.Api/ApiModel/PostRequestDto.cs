namespace Strider.Posterr.Api.ApiModel;

public class PostRequestApiModel
{
    public string Content { get; set; }
    public Guid? OriginalPostId { get; set; }
    public Guid? CreatedBy { get; set; }
}