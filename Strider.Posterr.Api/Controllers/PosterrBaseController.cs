using Microsoft.AspNetCore.Mvc;
using Strider.Posterr.Api.ApiModel;
using Strider.Posterr.Domain.Dtos;
using Strider.Posterr.Domain.Factory;
using Strider.Posterr.Domain.Services;

namespace Strider.Posterr.Api.Controllers;

public abstract class PosterrBaseController<T> : ControllerBase where T : ControllerBase
{
    private readonly ILogger<T> _logger;
    private readonly IPostService _postService;

    protected PosterrBaseController(ILogger<T> logger, IPostService postService)
    {
        _logger = logger;
        _postService = postService;
    }

    public virtual async Task<ObjectResult> CreatePostAsync(PostRequestApiModel request)
    {
        _logger.LogInformation("adding a new post");
        try
        {
            var postRequestDto = BuildRequestDto(request);
            var response = await _postService.AddAsync(postRequestDto);
            return response.Success ? new AcceptedResult() : new BadRequestObjectResult(response.ErrorMessage);
        }
        catch (Exception e)
        {
            _logger.LogError("error on adding a new post {@Exception}", e.Message);
            return new BadRequestObjectResult("Error on adding a new post. Contact the administrator");
        }
    }

    private static PostRequestDto BuildRequestDto(PostRequestApiModel request)
    {
        return new PostRequestDto(request.Content, request.OriginalPostId)
        {
            Id = Guid.NewGuid(),
            CreatedById = request.CreatedBy ?? UserFactory.UserOne().Id //ideally should get from jwt token
        };
    }
}