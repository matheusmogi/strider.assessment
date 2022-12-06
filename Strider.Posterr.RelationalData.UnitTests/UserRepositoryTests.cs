using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Strider.Posterr.Domain.Factory;
using Strider.Posterr.RelationalData.Repositories;

namespace Strider.Posterr.RelationalData.UnitTests;

[TestFixture]
public class UserRepositoryTests : RepositoryBaseTest
{
    private UserRepository _userRepository;

    [SetUp]
    protected override void AdditionalSetup()
    {
        _userRepository = new UserRepository(Context);
    }

    [TestCase(0, "User")]
    [TestCase(2, "User-One", "User-Two")]
    [TestCase(1, "User-One")]
    public async Task QueryByUserName_ReturnUsers(int usersCount, params string[] users)
    {
      await  Context.User.AddRangeAsync(UserFactory.UserOne(), UserFactory.UserTwo());
      await  Context.SaveChangesAsync();
        //act
        var actual = await _userRepository.QueryByUserNameAsync(users);

        //assert
        Assert.That(actual.Count(), Is.EqualTo(usersCount));
    }
}