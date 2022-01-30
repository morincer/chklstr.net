namespace Chklstr.Core.Utils;

public static class CollectionExtensions
{
    public static void AddAll<T>(this ICollection<T> collection, IEnumerable<T> newItems)
    {
        foreach (T item in newItems)
        {
            collection.Add(item);
        }
    }

    public static bool IsEmpty<T>(this ICollection<T> collection)
    {
        return collection.Count == 0;
    }
}