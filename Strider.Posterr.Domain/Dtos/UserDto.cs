using Strider.Posterr.Domain.Models;

namespace Strider.Posterr.Domain.Dtos;

public class UserDto
{
    public UserDto(User user)
    {
        if(user==null)
            return;
        Id = user.Id;
        UserName = user.UserName;
    }

    public Guid Id { get; set; }
    public string UserName { get; set; }
}