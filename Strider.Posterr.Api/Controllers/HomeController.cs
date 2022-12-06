using Microsoft.AspNetCore.Mvc;
using Strider.Posterr.Api.ApiModel;
using Strider.Posterr.Domain.Dtos;
using Strider.Posterr.Domain.Services;

namespace Strider.Posterr.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : PosterrBaseController<HomeController>
{
    private readonly IPostService _postService;

    public HomeController(ILogger<HomeController> logger, IPostService postService) : base(logger, postService)
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
    [Route("Posts")]
    public async Task<PostResponseDto> GetPostsAsync(Guid? userId, DateTime? from, DateTime? to, int offSet, int pageSize = 10)
    {
        var requestDto = new GetAllPostRequestDto { Offset = offSet, PageSize = pageSize, UserId = userId, From = from, To = to };
        return await _postService.GetAllPostsAsync(requestDto);
    }
}