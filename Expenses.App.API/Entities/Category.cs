using System.ComponentModel.DataAnnotations;
using Expenses.App.API.Entities;

namespace Expenses.App.API.Models;

public sealed class Category
{
    public string Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    // foreign key for USER -------------------------

    public string UserId { get; set; }

    // Relations ------------------------------------

    public List<CategoryTag> CategoryTags { get; set; }

    public List<Tag> Tags { get; set; }

    //public List<Product>? Products { get; set; }
}
