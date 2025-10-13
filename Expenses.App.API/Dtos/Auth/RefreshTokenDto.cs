namespace Expenses.App.API.Dtos.Auth;

public sealed record RefreshTokenDto
{
    public required string RefreshToken { get; init; }
}
