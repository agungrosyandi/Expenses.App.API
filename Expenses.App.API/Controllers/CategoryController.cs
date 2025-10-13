using System.Linq.Dynamic.Core;
using Expenses.App.API.Database;
using Expenses.App.API.Dtos.Categories;
using Expenses.App.API.Dtos.Categories.Mappings;
using Expenses.App.API.Dtos.Categories.Queries;
using Expenses.App.API.Entities;
using Expenses.App.API.Models;
using Expenses.App.API.Services;
using Expenses.App.API.Services.Sorting;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses.App.API.Controllers;

[ApiController]
[Route("category")]
[Authorize(Roles = Roles.Member)]
public sealed class CategoryController(ApplicationDbContext dbContext, UserContext userContext) : ControllerBase
{
    // ALL CATEGORY

    [HttpGet]
    public async Task<ActionResult<CategoryCollectionDto>> GetCategory([FromQuery] CategoryQueriesParameters query, SortMappingProvider sortMappingProvider)
    {
        if (!sortMappingProvider.ValidateMappings<CategoryDto, Category>(query.Sort))
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest,
                           detail: $"The provider sort Parameter isn't valid: {query.Sort}");
        }

        // query search --------------------------------------------------

        query.Search ??= query.Search?.Trim().ToLower();

        SortMapping[] sortMappings = sortMappingProvider.GetMappings<CategoryDto, Category>();

        List<CategoryDto> categories = await dbContext
                                             .Categories
                                             .Where(c => query.Search == null ||
                                                    c.Name.ToLower().Contains(query.Search) ||
                                                    c.Description != null && c.Description.ToLower().Contains(query.Search))
                                             .ApplySort(query.Sort, sortMappings)
                                             .Select(CategoryQueries.ProjectToDo())
                                             .ToListAsync();

        // -----------------------------------------------------------

        var categoryCollectionDto = new CategoryCollectionDto
        {
            Data = categories
        };

        return Ok(categoryCollectionDto);
    }

    // CATEGORY BY ID

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryWithTagsDto>> GetCategoryById(string id)
    {
        CategoryWithTagsDto categories = await dbContext.Categories
                                                        .Where(c => c.Id == id)
                                                        .Select(CategoryQueries.ProjectToDtoWithTags())
                                                        .FirstOrDefaultAsync();

        if (categories is null)
        {
            return NotFound();
        }

        return Ok(categories);
    }

    // CREATE NEW POST

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createCategoryDto, IValidator<CreateCategoryDto> validator)
    {
        // validations ----------------------

        await validator.ValidateAndThrowAsync(createCategoryDto);

        // Get the user ID from the UserContext service -----------------

        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        // process from DTO to Entity -----------------

        Category category = createCategoryDto.ToEntity();

        category.UserId = userId;

        dbContext.Categories.Add(category);

        await dbContext.SaveChangesAsync();

        CategoryDto categoryDto = category.ToDto();

        return CreatedAtAction(nameof(GetCategory), new { id = categoryDto.Id }, categoryDto);
    }

    // UPDATE POST

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCategory(string id, UpdateCategoryDto updateCategoryDto)
    {
        Category? category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
        {
            return NotFound();
        }

        category.UpdateFromDto(updateCategoryDto);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    // DELETE

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(string id)
    {
        Category? category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
        {
            return NotFound();
        }

        dbContext.Remove(category);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
