using System.Text.Json.Serialization;
using Expenses.App.API.Models.Base;

namespace Expenses.App.API.Models;

public class User : BaseEntity
{
    public string Name { get; set; }

    public string Email { get; set; }

    [JsonIgnore]
    public string Password { get; set; }

    // chid table relation, tied with transaction model

    [JsonIgnore]
    public virtual List<Transaction> Transactions { get; set; } = new();
}
