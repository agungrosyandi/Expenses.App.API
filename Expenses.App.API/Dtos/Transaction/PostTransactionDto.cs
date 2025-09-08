using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Expenses.App.API.Dtos.Transaction;

public class PostTransactionDto
{
    [Required]
    public Guid? PublicId { get; set; } // server generates, client can ignore

    [Required]
    public string Type { get; set; } = string.Empty;

    [JsonRequired]
    public double Amount { get; set; }

    [Required]
    public string Category { get; set; } = string.Empty;
}
