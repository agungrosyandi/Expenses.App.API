using System.Linq.Expressions;
using Expenses.App.API.Models;

namespace Expenses.App.API.Dtos.Users;

internal static class UserQueries
{
    public static Expression<Func<User, UserDto>> ProjectToDo()
    {
        return u => new UserDto
        {
            Id = u.Id,
            Email = u.Email,
            Name = u.Name,
            CreateAtUtc = u.CreateAtUtc,
            UpdateAtUtc = u.UpdateAtUtc,
        };
    }
}
