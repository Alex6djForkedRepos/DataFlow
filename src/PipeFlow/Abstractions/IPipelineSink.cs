namespace PipeFlow.Abstractions;

/// <summary>
/// A consumer for a pipeline's output. Implementations enumerate the source
/// stream exactly once and are responsible for their own flushing/disposal.
/// </summary>
/// <typeparam name="T">Row/record type consumed by the sink.</typeparam>
public interface IPipelineSink<in T>
{
    /// <summary>
    /// Consume items from the pipeline. Must drain <paramref name="source"/> to completion
    /// (respecting <paramref name="cancellationToken"/>) before the returned task completes.
    /// </summary>
    Task WriteAsync(IAsyncEnumerable<T> source, CancellationToken cancellationToken = default);
}
