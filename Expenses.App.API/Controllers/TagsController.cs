using Expenses.App.API.Database;
using Expenses.App.API.Dtos.Collections;
using Expenses.App.API.Dtos.Common;
using Expenses.App.API.Dtos.Tags;
using Expenses.App.API.Entities;
using Expenses.App.API.Models;
using Expenses.App.API.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses.App.API.Controllers;

[ApiController]
[Route("tags")]
[Authorize(Roles = Roles.Member)]
public class TagsController(ApplicationDbContext dbContext, UserContext userContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<TagsCollectionDto>> GetTags()
    {
        // Get the user ID from the UserContext service -----------------

        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        // -------------------------------------------------------------

        List<TagDto> tags = await dbContext.Tags.Select(TagQueries.ProjectToDto()).ToListAsync();

        var categoryCollectionDto = new TagsCollectionDto
        {
            Items = tags
        };

        return Ok(categoryCollectionDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDto>> GetTag(string id)
    {
        // Get the user ID from the UserContext service -----------------

        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        TagDto? tag = await dbContext.Tags.Where(c => c.Id == id && c.UserId == userId)
                                          .Select(TagQueries.ProjectToDto())
                                          .FirstOrDefaultAsync();

        if (tag is null)
        {
            return NotFound();
        }

        return Ok(tag);
    }

    [HttpPost]
    public async Task<ActionResult<TagDto>> CreateTag(CreateTagDto createTagDto, IValidator<CreateTagDto> validator)
    {
        // Get the user ID from the UserContext service -----------------

        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        // validations ------------------------------------------------

        ValidationResult validationResult = await validator.ValidateAsync(createTagDto);

        if (!validationResult.IsValid)
        {
            ProblemDetails problem = ProblemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status400BadRequest);

            problem.Extensions.Add("Error", validationResult.ToDictionary());

            return BadRequest(problem);
        }

        // -------------------------------------------------------------------------

        Tag tag = createTagDto.ToEntity();

        tag.UserId = userId;

        if (await dbContext.Tags.AnyAsync(t => t.Name == tag.Name))
        {
            return Problem(detail: $"The Tag {tag.Name} already exist", statusCode: StatusCodes.Status409Conflict);
        }

        dbContext.Tags.Add(tag);

        await dbContext.SaveChangesAsync();

        TagDto tagDto = tag.ToDto();

        return CreatedAtAction(nameof(GetTag), new { id = tagDto.Id }, tagDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTag(string id, UpdateTagDto updateTagDto)
    {
        // Get the user ID from the UserContext service -----------------

        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        // -------------------------------------------------------------------------

        Tag? tag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (tag is null)
        {
            return NotFound();
        }

        tag.UpdateFromDto(updateTagDto);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTag(string id)
    {
        // Get the user ID from the UserContext service -----------------

        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        // -------------------------------------------------------------------------

        Tag? tag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (tag is null)
        {
            return NotFound();
        }

        dbContext.Remove(tag);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
