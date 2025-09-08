using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Expenses.App.API.Dtos.Transaction;

public class PutTransactionDto
{
    // Make it nullable since client may omit it.
    public Guid? PublicId { get; set; }

    [Required]
    public string Type { get; set; } = string.Empty;

    [JsonRequired]  // <-- Sonar accepts this
    public double Amount { get; set; }

    [Required]
    public string Category { get; set; } = string.Empty;
}
