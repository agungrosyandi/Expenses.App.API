using System.Text.Json.Serialization;
using Expenses.App.API.Models.Enum;

namespace Expenses.App.API.Models;

public sealed class User
{
    public string Id { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public DateTime CreateAtUtc { get; set; }

    public DateTime? UpdateAtUtc { get; set; }

    public string IdentityId { get; set; }
}
