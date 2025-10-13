using Expenses.App.API.Models;

namespace Expenses.App.API.Dtos.Categories;

public sealed record CreateCategoryDto
{
    public required string Name { get; init; } = string.Empty;

    public required string Description { get; init; } = string.Empty;
}
