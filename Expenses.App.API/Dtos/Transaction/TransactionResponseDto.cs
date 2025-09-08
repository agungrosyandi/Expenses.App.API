using Expenses.App.API.Dtos.auth;

namespace Expenses.App.API.Dtos.Transaction;

public class TransactionResponseDto
{
    public string Type { get; set; }
    public double Amount { get; set; }
    public string Category { get; set; }
    public int UserId { get; set; }
    public UserResponseDto User { get; set; }
    public int Id { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
}
