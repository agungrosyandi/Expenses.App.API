using Expenses.App.API.Models.Base;

namespace Expenses.App.API.Models;

public class Transaction : BaseEntity
{
    public string Type { get; set; }

    public double Amount { get; set; }

    public string Category { get; set; }

    // connect with parent user -----------------------------------------------

    public int UserId { get; set; }

    public virtual User User { get; set; }
}
