namespace Expenses.App.API.Dtos.Users;

public sealed record class UserDto
{
    public required string Id { get; set; }

    public required string Email { get; set; }

    public required string Name { get; set; }

    public required DateTime CreateAtUtc { get; set; }

    public DateTime? UpdateAtUtc { get; set; }
}
