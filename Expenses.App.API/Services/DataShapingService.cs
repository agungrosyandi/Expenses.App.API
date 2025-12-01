//using System.Collections.Concurrent;
//using System.Dynamic;
//using System.Reflection;

//namespace Expenses.App.API.Services;

//public sealed class DataShapingService
//{
//    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertiesCache = new();

//    public List<ExpandoObject> ShapeDate<T>(IEnumerable<T> entities, string? fields)
//    {
//        var fieldSet = fields.Split(",", StringSplitOptions.RemoveEmptyEntries)
//                             .Select(f => f.Trim())
//                             .ToHashSet(StringComparer.OrdinalIgnoreCase);

//        PropertyInfo[] propertyInfos = PropertiesCache.GetOrAdd(typeof(T), t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance));
//    }
//}
