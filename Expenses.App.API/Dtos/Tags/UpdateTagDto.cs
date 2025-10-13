namespace Expenses.App.API.Dtos.Tags;

public sealed record UpdateTagDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
