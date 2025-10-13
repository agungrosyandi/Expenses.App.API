using Microsoft.AspNetCore.Identity;

namespace Expenses.App.API.Models;

public sealed class RefreshToken
{
    public Guid Id { get; set; }

    public string UserId { get; set; }

    public string Token { get; set; }

    public DateTime ExpiresAtUtc { get; set; }

    public IdentityUser User { get; set; }
}
