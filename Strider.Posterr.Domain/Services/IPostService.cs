using Strider.Posterr.Domain.Dtos;

namespace Strider.Posterr.Domain.Services;

public interface IPostService
{
    Task<PostCreateResponse> AddAsync(PostRequestDto postRequestDto);
    Task<PostResponseDto> GetPostsByUserAsync(GetPostRequestDto postRequestDto);
    Task<PostResponseDto> GetAllPostsAsync(GetAllPostRequestDto postRequestDto);
}