using Expenses.App.API.Models;

namespace Expenses.App.API.Dtos.Categories;

public sealed record CategoryWithTagsDto
{
    public required string Id { get; init; }

    public required string Name { get; init; } = string.Empty;

    public required string Description { get; init; } = string.Empty;

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; init; }

    public required string[] Tags { get; init; }
}
