using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Strider.Posterr.Domain.Dtos;
using Strider.Posterr.Domain.Factory;
using Strider.Posterr.Domain.Models;
using Strider.Posterr.Domain.Repositories;
using Strider.Posterr.RelationalData.Repositories;

namespace Strider.Posterr.RelationalData.UnitTests;

[TestFixture]
public class PostRepositoryTests : RepositoryBaseTest
{
    private IPostRepository _postRepository;
    private static readonly User User = UserFactory.UserOne();
    private static readonly User AnotherUser = UserFactory.UserTwo();
    private static readonly List<Post> PostEntities = BuildPosts();

    [SetUp]
    protected override void AdditionalSetup()
    {
        _postRepository = new PostRepository(Context);
    }

    [Test]
    public async Task AddAsync_ShouldAddPost()
    {
        // arrange
        var createdBy = User;
        var entity = new Post(Guid.NewGuid(), createdBy.Id, "test", null, null);

        // act
        await _postRepository.AddAsync(entity);
        await Context.SaveChangesAsync();

        // assert
        var post = await _postRepository.GetByIdAsync(entity.Id);
        Assert.Multiple(() =>
        {
            Assert.That(entity.Content, Is.EqualTo(post!.Content));
            Assert.That(entity.Id, Is.EqualTo(post.Id));
        });
    }

    [Test]
    public async Task GetPostsByUserAsync_ShouldReturnPosts()
    {
        // arrange 
        await Context.AddRangeAsync(PostEntities);
        await Context.SaveChangesAsync();

        // act
        var posts = (await _postRepository.GetPostsByUserAsync(User.Id, 10, 0)).Results;

        // assert
        Assert.That(posts.Count, Is.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(posts[0].Content, Is.EqualTo(PostEntities[1].Content));
            Assert.That(posts[0].Id, Is.EqualTo(PostEntities[1].Id));
            Assert.That(posts[1].Content, Is.EqualTo(PostEntities[2].Content));
            Assert.That(posts[1].Id, Is.EqualTo(PostEntities[2].Id));
            Assert.That(posts[2].Content, Is.EqualTo(PostEntities[3].Content));
            Assert.That(posts[2].Id, Is.EqualTo(PostEntities[3].Id));
            Assert.That(posts[3].Content, Is.EqualTo(PostEntities[4].Content));
            Assert.That(posts[3].Id, Is.EqualTo(PostEntities[4].Id));
        });
    }

    [Test]
    public async Task GetAllPostsAsync_ShouldReturnPosts()
    {
        // arrange 
        await Context.AddRangeAsync(PostEntities);
        await Context.SaveChangesAsync();
        var requestDto = new GetAllPostRequestDto
        {
            Offset = 0,
            PageSize = 5
        };

        // act
        var posts = (await _postRepository.GetAllPostsAsync(requestDto)).Results;

        // assert
        Assert.That(posts.Count, Is.EqualTo(5));
        Assert.Multiple(() =>
        {
            Assert.That(posts[0].Content, Is.EqualTo(PostEntities[0].Content));
            Assert.That(posts[0].Id, Is.EqualTo(PostEntities[0].Id));
            Assert.That(posts[1].Content, Is.EqualTo(PostEntities[1].Content));
            Assert.That(posts[1].Id, Is.EqualTo(PostEntities[1].Id));
            Assert.That(posts[2].Content, Is.EqualTo(PostEntities[2].Content));
            Assert.That(posts[2].Id, Is.EqualTo(PostEntities[2].Id));
            Assert.That(posts[3].Content, Is.EqualTo(PostEntities[3].Content));
            Assert.That(posts[3].Id, Is.EqualTo(PostEntities[3].Id));
        });
    }

    [Test]
    public async Task GetAllPostsAsync_WithDateRange_ShouldReturnPosts()
    {
        // arrange 
        await Context.AddRangeAsync(PostEntities);
        await Context.SaveChangesAsync();
        var requestDto = new GetAllPostRequestDto
        {
            Offset = 0,
            PageSize = 5,
            From = DateTime.Today.AddDays(-2),
            To = DateTime.Today,
        };

        // act
        var posts = (await _postRepository.GetAllPostsAsync(requestDto)).Results;

        // assert
        Assert.That(posts.Count, Is.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(posts[0].Content, Is.EqualTo(PostEntities[0].Content));
            Assert.That(posts[0].Id, Is.EqualTo(PostEntities[0].Id));
            Assert.That(posts[1].Content, Is.EqualTo(PostEntities[1].Content));
            Assert.That(posts[1].Id, Is.EqualTo(PostEntities[1].Id));
            Assert.That(posts[2].Content, Is.EqualTo(PostEntities[2].Content));
            Assert.That(posts[2].Id, Is.EqualTo(PostEntities[2].Id));
        });
    }

    [Test]
    public async Task GetAllPostsAsync_WithUserId_ShouldReturnOnlyUsersPosts()
    {
        // arrange 
        await Context.AddRangeAsync(PostEntities);
        await Context.SaveChangesAsync();
        var requestDto = new GetAllPostRequestDto
        {
            Offset = 0,
            PageSize = 5,
            UserId = UserFactory.UserOne().Id
        };

        // act
        var posts = (await _postRepository.GetAllPostsAsync(requestDto)).Results;

        // assert
        Assert.That(posts.Count, Is.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(posts[0].Content, Is.EqualTo(PostEntities[1].Content));
            Assert.That(posts[0].Id, Is.EqualTo(PostEntities[1].Id));
            Assert.That(posts[1].Content, Is.EqualTo(PostEntities[2].Content));
            Assert.That(posts[1].Id, Is.EqualTo(PostEntities[2].Id));
            Assert.That(posts[2].Content, Is.EqualTo(PostEntities[3].Content));
            Assert.That(posts[2].Id, Is.EqualTo(PostEntities[3].Id));
            Assert.That(posts[3].Content, Is.EqualTo(PostEntities[4].Content));
            Assert.That(posts[3].Id, Is.EqualTo(PostEntities[4].Id));
        });
    }

    [TestCaseSource(nameof(GetAllPostsByUserAsyncPaginationDataSource))]
    public async Task GetAllPostsAsync_WithPagination_ShouldReturnPostsPaginated(int pageSize, int offSet, IEnumerable<Post> expected)
    {
        // arrange  
        await Context.AddRangeAsync(PostEntities);
        await Context.SaveChangesAsync();
        var requestDto = new GetAllPostRequestDto
        {
            Offset = offSet,
            PageSize = pageSize
        };

        // act
        var posts = await _postRepository.GetAllPostsAsync(requestDto);
        Assert.Multiple(() =>
        {
            // assert
            Assert.That(posts.TotalRecords, Is.EqualTo(5));
            Assert.That(expected, Is.EquivalentTo(posts.Results));
        });
    }

    [TestCaseSource(nameof(GetPostsByUserAsyncPaginationDataSource))]
    public async Task GetPostsByUserAsync_WithPagination_ShouldReturnPostsPaginated(int pageSize, int offSet, IEnumerable<Post> expected)
    {
        // arrange  
        await Context.AddRangeAsync(PostEntities);
        await Context.SaveChangesAsync();

        // act
        var posts = await _postRepository.GetPostsByUserAsync(User.Id, pageSize, offSet);
        Assert.Multiple(() =>
        {
            // assert
            Assert.That(posts.TotalRecords, Is.EqualTo(4));
            Assert.That(expected, Is.EquivalentTo(posts.Results));
        });
    }

    private static List<Post> BuildPosts()
    {
        var anotherUserPost = new Post(Guid.NewGuid(), AnotherUser.Id, "anotherUserPost", null) { CreatedOn = DateTime.Now };
        var originalPost = new Post(Guid.NewGuid(), User.Id, "original", null) { CreatedOn = DateTime.Now.AddDays(-1) };
        var entity = new Post(Guid.NewGuid(), User.Id, "test-1", null, AnotherUser) { CreatedOn = DateTime.Now.AddDays(-2) };
        var anotherEntity = new Post(Guid.NewGuid(), AnotherUser.Id, "test-2", null, User) { CreatedOn = DateTime.Now.AddDays(-3) };
        var repost = new Post(Guid.NewGuid(), AnotherUser.Id, "repost", originalPost.Id) { OriginalPost = originalPost, CreatedOn = DateTime.Now.AddDays(-4) };

        return new List<Post>
        {
            anotherUserPost, originalPost, entity, anotherEntity, repost
        };
    }

    private static IEnumerable<TestCaseData> GetPostsByUserAsyncPaginationDataSource()
    {
        yield return new TestCaseData(2, 0, new[] { PostEntities[1], PostEntities[2] });
        yield return new TestCaseData(2, 1, new[] { PostEntities[2], PostEntities[3] });
        yield return new TestCaseData(3, 0, new[] { PostEntities[1], PostEntities[2], PostEntities[3] });
        yield return new TestCaseData(5, 4, Enumerable.Empty<Post>());
    }

    private static IEnumerable<TestCaseData> GetAllPostsByUserAsyncPaginationDataSource()
    {
        yield return new TestCaseData(2, 0, new[] { PostEntities[0], PostEntities[1] });
        yield return new TestCaseData(2, 1, new[] { PostEntities[1], PostEntities[2] });
        yield return new TestCaseData(3, 0, new[] { PostEntities[0], PostEntities[1], PostEntities[2] });
        yield return new TestCaseData(5, 5, Enumerable.Empty<Post>());
    }
}