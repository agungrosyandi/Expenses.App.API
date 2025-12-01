namespace Expenses.App.API.Dtos.Token;

public sealed record RefreshTokenDto
{
    public required string RefreshToken { get; init; }
}
