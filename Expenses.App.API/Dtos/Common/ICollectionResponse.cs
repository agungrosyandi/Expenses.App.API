namespace Expenses.App.API.Dtos.Common;

public interface ICollectionResponse<T>
{
    List<T> Items { get; init; }
}
