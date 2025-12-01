namespace Expenses.App.API.Dtos.Token;

public sealed record TokenRequest(string UserId, string Email, IEnumerable<string> Roles);
