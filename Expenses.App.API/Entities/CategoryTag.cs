namespace Expenses.App.API.Entities;

public sealed class CategoryTag
{
    public string CategoryId { get; set; }

    public string TagId { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
