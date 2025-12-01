using Expenses.App.API.Dtos.Common;
using Expenses.App.API.Dtos.Tags;

namespace Expenses.App.API.Dtos.Collections;

public sealed record TagsCollectionDto : ICollectionResponse<TagDto>
{
    public List<TagDto> Items { get; init; }
}
