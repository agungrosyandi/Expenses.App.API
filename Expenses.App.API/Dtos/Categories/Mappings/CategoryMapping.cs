using Expenses.App.API.Models;
using Expenses.App.API.Services.Sorting;

namespace Expenses.App.API.Dtos.Categories.Mappings;

internal static class CategoryMapping
{
    // SORT MAPPING

    public static readonly SortMappingDefinition<CategoryDto, Category> SortMapping = new()
    {
        Mappings =
        [
            new SortMapping(nameof(CategoryDto.Name), nameof(Category.Name)),
            new SortMapping(nameof(CategoryDto.Description), nameof(Category.Description))
        ]
    };

    // CATEGORY DTO

    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAtUtc = category.CreatedAtUtc,
            UpdatedAtUtc = category.UpdatedAtUtc,
        };
    }

    // CONVERT CATEGORY DTO TO ENTITY

    public static Category ToEntity(this CreateCategoryDto dto)
    {
        Category category = new()
        {
            Id = $"h_{Guid.CreateVersion7()}",
            Name = dto.Name,
            Description = dto.Description,
            CreatedAtUtc = DateTime.UtcNow,
        };

        return category;
    }

    // UPDATE FROM DTO

    public static void UpdateFromDto(this Category category, UpdateCategoryDto dto)
    {
        category.Name = dto.Name;
        category.Description = dto.Description;
        category.UpdatedAtUtc = DateTime.UtcNow;
    }
}
