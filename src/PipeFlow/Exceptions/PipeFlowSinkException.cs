#pragma warning disable CA1032 // Intentional: callers must supply SinkType context.
namespace PipeFlow.Exceptions;

/// <summary>
/// Thrown when a pipeline sink fails to consume data (write error, constraint violation,
/// connection failure, etc.).
/// </summary>
public sealed class PipeFlowSinkException : PipeFlowException
{
    /// <summary>Kind of sink that failed (e.g., "Csv", "SqlServer", "Http").</summary>
    public string SinkType { get; }

    /// <summary>Sink-specific location. May be null.</summary>
    public string? Location { get; }

    public PipeFlowSinkException(string sinkType, string message)
        : base(message)
    {
        SinkType = sinkType;
    }

    public PipeFlowSinkException(string sinkType, string? location, string message,
        Exception? innerException = null)
        : base(message, innerException)
    {
        SinkType = sinkType;
        Location = location;
    }
}
