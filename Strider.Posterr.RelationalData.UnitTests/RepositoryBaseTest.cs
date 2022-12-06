using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Strider.Posterr.RelationalData.UnitTests;

[TestFixture]
public abstract class RepositoryBaseTest
{
    protected PosterrSqlDbContext Context;

    [SetUp]
    public void Setup()
    {
       var dbContextOptions = new DbContextOptionsBuilder<PosterrSqlDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryPosterr")
            .Options;
        Context = new PosterrSqlDbContext(dbContextOptions);
        AdditionalSetup();
    }

    protected virtual void AdditionalSetup()
    {
    }
    
    
    [TearDown]
    public async Task TearDown()
    {
        foreach (var post in Context.Post)
        {
            Context.Post.Remove(post);
        }

        foreach (var user in Context.User)
        {
            Context.User.Remove(user);
        }

        await Context.SaveChangesAsync();
    }
}