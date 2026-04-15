using System.Linq.Expressions;

namespace PipeFlow;

/// <summary>
/// Specialized pipeline for <see cref="IQueryable{T}"/>-backed sources (Entity Framework Core).
/// Accepts <see cref="Expression{TDelegate}"/> overloads of <c>Where</c>/<c>Select</c>/<c>OrderBy</c>
/// so operations translate to SQL instead of client-side filtering. Enumeration is deferred
/// until a terminal call.
/// </summary>
/// <remarks>
/// Calling <see cref="IPipeline{T}.Chunk"/> on a queryable pipeline forces client-side
/// materialization (chunk has no SQL translation) and returns a non-queryable
/// <see cref="IPipeline{T}"/>.
/// </remarks>
public interface IQueryablePipeline<T> : IPipeline<T>
{
    IQueryablePipeline<T> Where(Expression<Func<T, bool>> predicate);
    // CA1716: 'Select' matches a VB/other-lang reserved keyword; name is intentional LINQ parity.
#pragma warning disable CA1716
    IQueryablePipeline<TResult> Select<TResult>(Expression<Func<T, TResult>> selector);
#pragma warning restore CA1716
    IOrderedQueryablePipeline<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector);
    IOrderedQueryablePipeline<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector);

    /// <summary>Enable keyset-style server-side paging with the given page size.</summary>
    IQueryablePipeline<T> WithPaging(int pageSize);

    /// <summary>Entity Framework no-tracking hint for read-only queries.</summary>
    IQueryablePipeline<T> AsNoTracking();
}
