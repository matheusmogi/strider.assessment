using Moq;
using NUnit.Framework;
using Strider.Posterr.Common;
using Strider.Posterr.Domain.Dtos;
using Strider.Posterr.Domain.Models;
using Strider.Posterr.Domain.Repositories;
using Strider.Posterr.Domain.Services;

namespace Strider.Posterr.Service.UnitTests;

public class PostServiceTests
{
    private IPostService _service;
    private Mock<IPostRepository> _postRepository;
    private Mock<IUserRepository> _userRepository;
    private Mock<IUnitOfWork> _unitOfWork;

    [SetUp]
    public void Setup()
    {
        _postRepository = new Mock<IPostRepository>();
        _userRepository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _service = new PostService(_postRepository.Object, _userRepository.Object, _unitOfWork.Object);
    }

    [Test]
    public async Task AddAsync_HappyPath()
    {
        // arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "user"
        };
        _userRepository.Setup(a => a.QueryByUserNameAsync(new[] { "user" })).ReturnsAsync(new[]
        {
            user
        }.AsEnumerable());
        var postRequestDto = new PostRequestDto("test @user", null)
        {
            CreatedById = UserFactory.UserOne().Id,
        };

        // act
        await _service.AddAsync(postRequestDto);

        // assert
        _postRepository.Verify(s => s.AddAsync(It.Is<Post>(p =>
            p.Id == postRequestDto.Id &&
            p.Content == postRequestDto.Content &&
            p.CreatedById == postRequestDto.CreatedById &&
            p.OriginalPostId == postRequestDto.OriginalPostId &&
            p.UsersMentioned!.Any(a => a.Id == user.Id)
        )));
        _unitOfWork.Verify(s => s.SaveChangesAsync());
    }

    [Test]
    public async Task AddAsync_WithUsersMentioned_ShouldExtractUsers()
    {
        // arrange
        var user = UserFactory.UserOne();
        var user2 = UserFactory.UserTwo();

        _userRepository.Setup(a => a.QueryByUserNameAsync(new[] { "user", "user.2" })).ReturnsAsync(new[]
        {
            user, user2
        }.AsEnumerable());
        var postRequestDto = new PostRequestDto("test @user @user.2", null)
        {
            CreatedById = user.Id,
        };

        // act
        await _service.AddAsync(postRequestDto);

        // assert
        _postRepository.Verify(s => s.AddAsync(It.Is<Post>(p =>
            p.UsersMentioned!.All(a => a.Id == user.Id || a.Id == user2.Id)
        )));
    }

    [Test]
    public async Task AddAsync_WithContentTooLong_ReturnsError()
    {
        // arrange
        var user = UserFactory.UserOne();

        var postRequestDto = new PostRequestDto(@"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec eu varius lectus. Aenean nec hendrerit 
mauris. Aenean a erat sit amet nisl pellentesque laoreet. Pellentesque nec elementum nulla, sit amet porta sem. Quisque suscipit ultricies metus sit amet 
euismod. Pellentesque rhoncus varius neque eget porttitor. Nullam eu metus eu turpis gravida ultrices at eget quam. Curabitur urna metus, congue et hendrerit 
at, tempor non nisi. Nullam facilisis at risus a varius. Nam arcu augue, commodo lobortis massa ut, consequat eleifend enim. Ut vehicula ultrices vulputate.
Donec malesuada augue eu eros scelerisque posuere. Suspendisse ante ex, finibus vel semper sed, aliquet id tellus. Sed posuere eleifend vulputate. Vivamus 
vulputate leo sed volutpat lacinia. Etiam eu eleifend erat. Maecenas luctus risus orci, at maximus turpis blandit sed. Quisque lobortis imperdiet ultrices.
Pellentesque sed urna purus. Vivamus iaculis ornare vehicula. Nunc maximus accumsan elit, in convallis metus varius non. Duis lacinia mi ut purus vestibulum 
porta. Integer dapibus tellus nec lacus elementum, ac pretium quam faucibus. Integer fringilla at mauris a blandit. Pellentesque a arcu imperdiet, ullamcorper 
ligula et, pharetra odio. Pellentesque non tincidunt nulla, eu iaculis enim. Phasellus nec consectetur ligula.", null)
        {
            CreatedById = user.Id,
        };

        // act
        var actual = await _service.AddAsync(postRequestDto);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.Success, Is.False);
            Assert.That(actual.ErrorMessage, Is.EqualTo("Content too long."));
        });
    }

    [Test]
    public async Task AddAsync_WithoutUser_ReturnsError()
    {
        // arrange
        var postRequestDto = new PostRequestDto(@"test.", null);

        // act
        var actual = await _service.AddAsync(postRequestDto);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.Success, Is.False);
            Assert.That(actual.ErrorMessage, Is.EqualTo("An user creator is required."));
        });
    }

    [Test]
    public async Task AddAsync_WithoutPostLimitExceeded_ReturnsError()
    {
        // arrange
        var user = UserFactory.UserOne();
        var postRequestDto = new PostRequestDto(@"test.", null)
        {
            CreatedById = user.Id
        };
        _postRepository.Setup(a => a.GetTodayPostsByUser(user.Id)).Returns(5);
            
        // act
        var actual = await _service.AddAsync(postRequestDto);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.Success, Is.False);
            Assert.That(actual.ErrorMessage, Is.EqualTo("Your daily posts limit was reached"));
        });
    }

    [Test]
    public void GetPostsByUserAsync_WithoutUserId_ThrowsException()
    {
        // act
        var actual = Assert.ThrowsAsync<Exception>(async () => await _service.GetPostsByUserAsync(new GetPostRequestDto()));

        // assert
        Assert.That(actual.Message, Is.EqualTo("User id is required"));
    }

    [Test]
    public async Task GetPostsByUserAsync_HappyPath()
    {
        // arrange
        var user = UserFactory.UserOne();
        var requestDto = new GetPostRequestDto
        {
            UserId = user.Id,
            Offset = 5,
            PageSize = 5
        };
        var originalPostId = Guid.NewGuid();
        var originalPost = new Post(originalPostId, user.Id, "original post", null, null);
        _postRepository.Setup(a => a.GetPostsByUserAsync(requestDto.UserId, 5, 5)).ReturnsAsync(new QueryResult<Post>(
            new List<Post>
            {
                new(Guid.NewGuid(), user.Id, "post created by", null, null),
                new(Guid.NewGuid(), Guid.NewGuid(), "post mentioned", null, user),
                new(Guid.NewGuid(), Guid.NewGuid(), "post reposted", originalPost.Id, null)
                {
                    OriginalPost = originalPost
                },
            }, 4));


        // act
        var actual = await _service.GetPostsByUserAsync(requestDto);

        // assert
        var actualPosts = actual.Posts.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(actualPosts[0].Content, Is.EqualTo("post created by"));
            Assert.That(actualPosts[0].CreatedById, Is.EqualTo(user.Id));
            Assert.That(actualPosts[1].Content, Is.EqualTo("post mentioned"));
            Assert.That(actualPosts[1].UsersMentioned!.Any(a => a.Id == user.Id), Is.True);
            Assert.That(actualPosts[2].Content, Is.EqualTo("post reposted"));
            Assert.That(actualPosts[2].OriginalPostId, Is.EqualTo(originalPostId));
            Assert.That(actualPosts[2].OriginalPost!.Content, Is.EqualTo("original post"));
            Assert.That(actualPosts[2].OriginalPost!.CreatedById, Is.EqualTo(user.Id));
        });
    } 
    
    [Test]
    public async Task GetAllPostsAsync_HappyPath()
    {
        // arrange
        var userOne = UserFactory.UserOne();
        var userTwo = UserFactory.UserTwo();
        var requestDto = new GetAllPostRequestDto
        {
            Offset = 5,
            PageSize = 5,
            From = DateTime.Today.AddDays(-1),
            To = DateTime.Today.AddDays(1)
        };
        var originalPostId = Guid.NewGuid();
        var originalPost = new Post(originalPostId, userOne.Id, "original post", null, null);
        _postRepository.Setup(a => a.GetAllPostsAsync(It.IsAny<GetAllPostRequestDto>())).ReturnsAsync(new QueryResult<Post>(
            new List<Post>
            {
                new(Guid.NewGuid(), userTwo.Id, "another post", null, null),
                new(Guid.NewGuid(), userOne.Id, "post created by", null, null),
                new(Guid.NewGuid(), userTwo.Id, "post mentioned", null, userOne),
                new(Guid.NewGuid(), Guid.NewGuid(), "post reposted", originalPost.Id, null)
                {
                    OriginalPost = originalPost
                },
            }, 5));


        // act
        var actual = await _service.GetAllPostsAsync(requestDto);

        // assert
        var actualPosts = actual.Posts.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(actualPosts[0].Content, Is.EqualTo("another post"));
            Assert.That(actualPosts[0].CreatedById, Is.EqualTo(userTwo.Id));
            Assert.That(actualPosts[1].Content, Is.EqualTo("post created by"));
            Assert.That(actualPosts[1].CreatedById, Is.EqualTo(userOne.Id));
            Assert.That(actualPosts[2].Content, Is.EqualTo("post mentioned"));
            Assert.That(actualPosts[2].UsersMentioned!.Any(a => a.Id == userOne.Id), Is.True);
            Assert.That(actualPosts[3].Content, Is.EqualTo("post reposted"));
            Assert.That(actualPosts[3].OriginalPostId, Is.EqualTo(originalPostId)); 
        });
    }
}