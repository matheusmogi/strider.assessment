using System.Text.RegularExpressions;
using Strider.Posterr.Domain.Dtos;
using Strider.Posterr.Domain.Models;
using Strider.Posterr.Domain.Repositories;
using Strider.Posterr.Domain.Services;

namespace Strider.Posterr.Service;

public class PostService : IPostService
{
    private readonly IPostRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IPostRepository repository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PostCreateResponse> AddAsync(PostRequestDto postRequestDto)
    {
        var validator = new PostValidationBuilder(postRequestDto, _repository)
            .WithMaximumContent(777) //can be set on configurations
            .WithMaximumPostToday(5) //can be set on configurations
            .WithRequiredUserId()
            .Build();
        
        if (validator.Any())
        {
            //log errors
            return PostCreateResponse.WithError(validator.First().Value);
        }
        
        var mentionedUsers = await ExtractMentionedUsers(postRequestDto.Content);
        var postEntity = new Post(postRequestDto.Id, postRequestDto.CreatedById, postRequestDto.Content, postRequestDto.OriginalPostId, mentionedUsers);
        await _repository.AddAsync(postEntity);
        await _unitOfWork.SaveChangesAsync();

        return PostCreateResponse.WithSuccess();
    }

    public async Task<PostResponseDto> GetPostsByUserAsync(GetPostRequestDto postRequestDto)
    {
        if (postRequestDto.UserId == default)
        {
            throw new Exception("User id is required");
        }
        
        var queryResponse = await _repository.GetPostsByUserAsync(postRequestDto.UserId, postRequestDto.PageSize, postRequestDto.Offset);
        var entities = queryResponse.Results;
        return new PostResponseDto(entities, queryResponse.TotalRecords);
    }

    public async Task<PostResponseDto> GetAllPostsAsync(GetAllPostRequestDto postRequestDto)
    {
        var queryResponse = await _repository.GetAllPostsAsync(postRequestDto);
        var entities = queryResponse.Results;
        return new PostResponseDto(entities, queryResponse.TotalRecords);
    }

    private async Task<User[]> ExtractMentionedUsers(string content)
    {
        var regex = new Regex("@(?<name>[^\\s]+)");
        var matches = regex.Matches(content)
            .Select(m => m.Groups["name"].Value)
            .ToArray();
        return (await _userRepository.QueryByUserNameAsync(matches)).ToArray();
    }
}