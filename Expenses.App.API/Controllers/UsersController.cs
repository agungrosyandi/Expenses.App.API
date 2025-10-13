using System.Security.Claims;
using Expenses.App.API.Database;
using Expenses.App.API.Dtos.Users;
using Expenses.App.API.Entities;
using Expenses.App.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses.App.API.Controllers;

[ApiController]
[Route("users")]
[Authorize(Roles = Roles.Member)]
public sealed class UsersController(ApplicationDbContext dbContext, UserContext userContext) : ControllerBase
{
    // GET USER BY ID ------------------

    [HttpGet("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<UserDto>> GetUserById(string id)
    {
        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        if (id != userId)
        {
            return Forbid();
        }

        UserDto? user = await dbContext.Users
                                       .Where(u => u.Id == id)
                                       .Select(UserQueries.ProjectToDo())
                                       .FirstOrDefaultAsync();

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    // GET CURRENT USER WHO ALREADY LOGIN STATUS ------------------

    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        UserDto? user = await dbContext.Users
                                       .Where(u => u.Id == userId)
                                       .Select(UserQueries.ProjectToDo())
                                       .FirstOrDefaultAsync();

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}
