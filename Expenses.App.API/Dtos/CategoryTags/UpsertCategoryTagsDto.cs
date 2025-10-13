namespace Expenses.App.API.Dtos.CategoryTags;

public sealed record UpsertCategoryTagsDto
{
    public required List<string> TagIds { get; init; }
}
