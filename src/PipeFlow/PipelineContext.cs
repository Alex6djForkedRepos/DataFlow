using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace PipeFlow;

/// <summary>
/// Cross-cutting state carried by every pipeline operation. Includes logger,
/// options, HTTP factory, service provider, cancellation, and parallelism hint.
/// Immutable - operations that need to change the context produce a new one.
/// </summary>
/// <remarks>
/// Spec §6.3 declares <see cref="HttpClientFactory"/> and <see cref="Services"/> as positional
/// nullable parameters without defaults; this implementation adds <c>= null</c> defaults so
/// <see cref="Empty"/> and the static <c>PipeFlow.From</c> facade can construct a context with
/// only the required logger/options. Types are unchanged (<c>IHttpClientFactory?</c>, <c>IServiceProvider?</c>);
/// callers that supplied all six positional args continue to compile.
/// </remarks>
// CA1068: CancellationToken intentionally precedes MaxDegreeOfParallelism as specified in §6.3.
#pragma warning disable CA1068
public readonly record struct PipelineContext(
    ILogger Logger,
    PipeFlowOptions Options,
    IHttpClientFactory? HttpClientFactory = null,
    IServiceProvider? Services = null,
    CancellationToken CancellationToken = default,
    int? MaxDegreeOfParallelism = null)
#pragma warning restore CA1068
{
    /// <summary>
    /// A minimal context with <see cref="NullLogger.Instance"/> and default options. Suitable
    /// for tests and the static <c>PipeFlow.From</c> facade. Not in spec §6.3 but a pragmatic
    /// factory for zero-config entry points.
    /// </summary>
    public static PipelineContext Empty { get; } = new(NullLogger.Instance, new PipeFlowOptions());
}
