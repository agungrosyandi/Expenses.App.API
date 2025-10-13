using Expenses.App.API.Dtos.Auth;
using Expenses.App.API.Models;

namespace Expenses.App.API.Dtos.Users;

public static class UserMappings
{
    public static User ToEntity(this RegisterUserDto dto)
    {
        return new User
        {
            Id = $"u_{Guid.CreateVersion7()}",
            Name = dto.Name,
            Email = dto.Email,
            CreateAtUtc = DateTime.UtcNow
        };
    }
}
