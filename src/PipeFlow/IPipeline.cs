using PipeFlow.Abstractions;

namespace PipeFlow;

/// <summary>
/// A lazy, composable pipeline over <typeparamref name="T"/>. All I/O is async-first;
/// transformations are sync LINQ-style. Enumeration is driven by a terminal operation.
/// </summary>
/// <typeparam name="T">Row/record type flowing through the pipeline.</typeparam>
public interface IPipeline<T>
{
    /// <summary>Cross-cutting state (logger, options, CT, services).</summary>
    PipelineContext Context { get; }

    // Composition - lazy
    IPipeline<T> Where(Func<T, bool> predicate);
    IPipeline<T> Where(Func<T, int, bool> predicate);

    // CA1716: 'Select' matches a VB/other-lang reserved keyword; name is intentional LINQ parity.
#pragma warning disable CA1716
    IPipeline<TResult> Select<TResult>(Func<T, TResult> selector);
    IPipeline<TResult> Select<TResult>(Func<T, int, TResult> selector);
#pragma warning restore CA1716

    IPipeline<TResult> SelectMany<TResult>(Func<T, IEnumerable<TResult>> selector);
    IPipeline<TResult> SelectManyAsync<TResult>(
        Func<T, CancellationToken, IAsyncEnumerable<TResult>> selector);

    IPipeline<T> Take(int count);
    IPipeline<T> Skip(int count);
    IPipeline<T> TakeWhile(Func<T, bool> predicate);
    IPipeline<T> SkipWhile(Func<T, bool> predicate);

    IPipeline<T> Distinct(IEqualityComparer<T>? comparer = null);

    IOrderedPipeline<T> OrderBy<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer = null);
    IOrderedPipeline<T> OrderByDescending<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer = null);

    IPipeline<IGrouping<TKey, T>> GroupBy<TKey>(
        Func<T, TKey> keySelector, IEqualityComparer<TKey>? comparer = null);

    IPipeline<IReadOnlyList<T>> Chunk(int size);

    // Concurrency & binding
    IPipeline<T> AsParallel(int? maxDegreeOfParallelism = null);
    IPipeline<T> WithCancellation(CancellationToken cancellationToken);

    // Terminals
    IAsyncEnumerable<T> StreamAsync(CancellationToken ct = default);
    Task<IReadOnlyList<T>> ToListAsync(CancellationToken ct = default);
    Task<T[]> ToArrayAsync(CancellationToken ct = default);
    Task<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(Func<T, TKey> keySelector, CancellationToken ct = default)
        where TKey : notnull;

    Task<T> FirstAsync(CancellationToken ct = default);
    Task<T?> FirstOrDefaultAsync(CancellationToken ct = default);
    Task<T> SingleAsync(CancellationToken ct = default);
    Task<T?> SingleOrDefaultAsync(CancellationToken ct = default);

    Task<int> CountAsync(CancellationToken ct = default);
    Task<long> LongCountAsync(CancellationToken ct = default);

    Task<bool> AnyAsync(CancellationToken ct = default);
    Task<bool> AnyAsync(Func<T, bool> predicate, CancellationToken ct = default);
    Task<bool> AllAsync(Func<T, bool> predicate, CancellationToken ct = default);

    Task ForEachAsync(Func<T, CancellationToken, ValueTask> action, CancellationToken ct = default);

    Task WriteToAsync(IPipelineSink<T> sink, CancellationToken ct = default);
}
