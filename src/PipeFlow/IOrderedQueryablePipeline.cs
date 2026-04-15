using System.Linq.Expressions;

namespace PipeFlow;

/// <summary>
/// A queryable pipeline that has been ordered and can accept additional
/// <c>ThenBy</c>/<c>ThenByDescending</c> clauses.
/// </summary>
public interface IOrderedQueryablePipeline<T> : IQueryablePipeline<T>, IOrderedPipeline<T>
{
    IOrderedQueryablePipeline<T> ThenBy<TKey>(Expression<Func<T, TKey>> keySelector);
    IOrderedQueryablePipeline<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> keySelector);
}
