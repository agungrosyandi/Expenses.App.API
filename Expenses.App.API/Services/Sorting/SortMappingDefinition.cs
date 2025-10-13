namespace Expenses.App.API.Services.Sorting;

public sealed class SortMappingDefinition<TSource, TDestination> : ISortMappingDefinition
{
    public SortMapping[] Mappings { get; set; }
}
