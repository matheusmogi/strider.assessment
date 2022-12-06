using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Strider.Posterr.Api.ApiModel;
using Strider.Posterr.Api.Controllers;
using Strider.Posterr.Domain.Dtos;
using Strider.Posterr.Domain.Factory;
using Strider.Posterr.Domain.Models;
using Strider.Posterr.Domain.Services;

namespace Strider.Posterr.Api.UnitTests;

public class PostsControllerTests
{
    private UserProfileController _controller;
    private Mock<IPostService> _postService;

    [SetUp]
    public void Setup()
    {
        _postService = new Mock<IPostService>();
        _controller = new UserProfileController(
            new Mock<ILogger<UserProfileController>>().Object, _postService.Object);
    }

    [Test]
    public async Task CreateAsync_HappyPath_ReturnsAccepted()
    {
        //arrange
        var requestDto = new PostRequestApiModel
        {
            Content = "this is a test",
            OriginalPostId = Guid.NewGuid()
        };
        _postService.Setup(s => s.AddAsync(It.IsAny<PostRequestDto>())).ReturnsAsync(PostCreateResponse.WithSuccess);

        //act
        var response = await _controller.CreatePostAsync(requestDto);

        //assert
        _postService.Verify(s => s.AddAsync(It.Is<PostRequestDto>(a =>
            a.Content == requestDto.Content &&
            a.OriginalPostId == requestDto.OriginalPostId &&
            a.CreatedById == UserFactory.UserOne().Id
        )));
        Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.Accepted));
    }

    [Test]
    public async Task CreateAsync_WithError_ReturnsBadRequest()
    {
        //arrange
        var requestDto = new PostRequestApiModel
        {
            Content = "this is a test",
            OriginalPostId = Guid.NewGuid()
        };
        _postService.Setup(s => s.AddAsync(It.IsAny<PostRequestDto>())).ReturnsAsync(PostCreateResponse.WithError("any validation error"));

        //act
        var response = await _controller.CreatePostAsync(requestDto);

        //assert 
        Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        Assert.That(response.Value, Is.EqualTo("any validation error"));
    }

    [Test]
    public async Task CreateAsync_WithException_ReturnsBadRequest()
    {
        //arrange
        var requestDto = new PostRequestApiModel
        {
            Content = "this is a test",
            OriginalPostId = Guid.NewGuid()
        };
        _postService.Setup(s => s.AddAsync(It.IsAny<PostRequestDto>())).ThrowsAsync(new Exception("any exception"));

        //act
        var response = await _controller.CreatePostAsync(requestDto);

        //assert 
        Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        Assert.That(response.Value, Is.EqualTo("Error on adding a new post. Contact the administrator"));
    }

    [Test]
    public async Task GetByUserAsync_HappyPath()
    {
        //arrange
        const int offset = 10;
        const int pageSize = 5;
        var entity = new Post
        {
            Content = "post",
            Id = Guid.NewGuid(),
            CreatedById = UserFactory.UserOne().Id,
            CreatedOn = DateTime.Today,
            OriginalPost = new Post { Id = Guid.NewGuid() },
            UsersMentioned = new List<User> { UserFactory.UserTwo() }
        };
        _postService.Setup(a => a.GetPostsByUserAsync(It.IsAny<GetPostRequestDto>()))
            .ReturnsAsync(new PostResponseDto(new[] { entity }, 1));

        //act
        var response = await _controller.GetPostsByUserAsync(offset, pageSize);

        //assert
        _postService.Verify(s => s.GetPostsByUserAsync(It.Is<GetPostRequestDto>(a =>
            a.Offset == offset &&
            a.PageSize == pageSize &&
            a.UserId == UserFactory.UserOne().Id
        )));
        var actual = response.Posts.First();
        Assert.That(actual.Content, Is.EqualTo(entity.Content));
        Assert.That(actual.Id, Is.EqualTo(entity.Id));
        Assert.That(actual.CreatedById, Is.EqualTo(entity.CreatedById));
        Assert.That(actual.UsersMentioned[0].Id, Is.EqualTo(UserFactory.UserTwo().Id));
    }
}