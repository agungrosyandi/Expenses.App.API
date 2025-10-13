using Microsoft.AspNetCore.Mvc;

namespace Expenses.App.API.Dtos.Categories.Queries;

public sealed record CategoryQueriesParameters
{
    [FromQuery(Name = "q")]
    public string? Search { get; set; }

    public string? Sort { get; set; }

    public int? Page { get; set; } = 1;

    public int? PageSize { get; set; } = 10;
}
