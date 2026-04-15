namespace PipeFlow.Abstractions;

/// <summary>
/// A source of items for a pipeline. Implementations own the lifecycle of any
/// I/O resources (files, connections) via the enumerator returned from
/// <see cref="ReadAsync"/>; opening/closing happens as the consumer enumerates.
/// </summary>
/// <typeparam name="T">Row/record type produced by the source.</typeparam>
public interface IPipelineSource<out T>
{
    /// <summary>
    /// Produce items for the pipeline. The returned sequence is typically read once.
    /// </summary>
    IAsyncEnumerable<T> ReadAsync(CancellationToken cancellationToken = default);
}
