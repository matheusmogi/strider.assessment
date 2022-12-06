using Strider.Posterr.Domain.Models;

namespace Strider.Posterr.Domain.Factory;

/// <summary>
/// Class used to generate fake users
/// </summary>
public static class UserFactory
{
    public static User UserOne()
    {
        return new User
        {
            Id = Guid.Parse("EED1E1EB-51A2-4F33-9B97-401ACF10D5A8"),
            CreatedOn = new DateTime(2022, 01,01),
            UserName = "User-One"
        };
    }

    public static User UserTwo()
    {
        return new User
        {
            Id = Guid.Parse("215B5BD8-8D90-434B-B13F-1391DA1BB245"),
            CreatedOn = new DateTime(2022, 01,01),
            UserName = "User-Two"
        };
    }
}