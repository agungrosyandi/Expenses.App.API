using System.Linq.Expressions;
using Expenses.App.API.Models;

namespace Expenses.App.API.Dtos.Categories.Queries;

internal static class CategoryQueries
{
    // CATEGORY ENTITY & CATEGORY DTO

    public static Expression<Func<Category, CategoryDto>> ProjectToDo()
    {
        return c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            CreatedAtUtc = c.CreatedAtUtc,
            UpdatedAtUtc = c.UpdatedAtUtc,
        };
    }

    // CATEGORY ENTITY & TAG DTO

    public static Expression<Func<Category, CategoryWithTagsDto>> ProjectToDtoWithTags()
    {
        return c => new CategoryWithTagsDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            CreatedAtUtc = c.CreatedAtUtc,
            UpdatedAtUtc = c.UpdatedAtUtc,
            Tags = c.Tags.Select(t => t.Name).ToArray()
        };
    }
}
