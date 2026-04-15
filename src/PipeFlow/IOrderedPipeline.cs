namespace PipeFlow;

/// <summary>
/// Represents a pipeline whose items have been sorted. Enables <c>ThenBy</c>/<c>ThenByDescending</c>.
/// </summary>
public interface IOrderedPipeline<T> : IPipeline<T>
{
    IOrderedPipeline<T> ThenBy<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer = null);
    IOrderedPipeline<T> ThenByDescending<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer = null);
}
