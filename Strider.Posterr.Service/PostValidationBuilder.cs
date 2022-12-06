using Strider.Posterr.Domain.Dtos;
using Strider.Posterr.Domain.Repositories;

namespace Strider.Posterr.Service;

public class PostValidationBuilder
{
    private readonly PostRequestDto _postRequestDto;
    private readonly IPostRepository _repository;
    private readonly Dictionary<string, string> _errors = new();

    public PostValidationBuilder(PostRequestDto postRequestDto, IPostRepository repository)
    {
        _postRequestDto = postRequestDto;
        _repository = repository;
    }

    public PostValidationBuilder WithMaximumContent(int maxLenght)
    {
        if (_postRequestDto.Content.Length > maxLenght)
        {
            _errors.Add(nameof(_postRequestDto.Content), "Content too long.");
        }

        return this;
    }

    public PostValidationBuilder WithRequiredUserId()
    {
        if (_postRequestDto.CreatedById == default)
        {
            _errors.Add(nameof(_postRequestDto.CreatedById), "An user creator is required.");
        }

        return this;
    }

    public PostValidationBuilder WithMaximumPostToday(int maximumPostsPerDay)
    {
        if (_repository.GetTodayPostsByUser(_postRequestDto.CreatedById) >= maximumPostsPerDay)
        {
            _errors.Add("maximum-exceeded", "Your daily posts limit was reached");
        }

        return this;
    }

    public Dictionary<string, string> Build()
    {
        return _errors;
    }
}