namespace Expenses.App.API.Dtos.Auth;

public sealed record TokenRequest(string UserId, string Email, IEnumerable<string> Roles);
