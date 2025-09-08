using System.Text.Json.Serialization;

namespace Expenses.App.API.Models.Base;

public class BaseEntity
{
    public int Id { get; set; } // Primary Key for DB

    [JsonIgnore]
    public Guid PublicId { get; set; } = Guid.NewGuid(); // Exposed to frontend

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }
}
