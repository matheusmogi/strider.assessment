using Microsoft.AspNetCore.Mvc;
using Strider.Posterr.Api.ApiModel;
using Strider.Posterr.Domain.Dtos;
using Strider.Posterr.Domain.Factory;
using Strider.Posterr.Domain.Services;

namespace Strider.Posterr.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserProfileController : PosterrBaseController<UserProfileController>
{
    private readonly IPostService _postService;

    public UserProfileController(ILogger<UserProfileController> logger, IPostService postService) : base(logger, postService)
    {
        _postService = postService;
    }
    
    [HttpPost]
    [Route("CreatePost")]
    public override Task<ObjectResult> CreatePostAsync(PostRequestApiModel request)
    {
        return base.CreatePostAsync(request);
    }

    [HttpGet]
    [Route("Posts/ByUser")]
    public async Task<PostResponseDto> GetPostsByUserAsync(int offSet, int pageSize = 5)
    {
        var requestDto = new GetPostRequestDto { Offset = offSet, PageSize = pageSize, UserId = UserFactory.UserOne().Id };
        return await _postService.GetPostsByUserAsync(requestDto);
    }
}