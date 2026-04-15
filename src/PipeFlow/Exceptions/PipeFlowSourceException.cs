#pragma warning disable CA1032 // Intentional: callers must supply SourceType context.
namespace PipeFlow.Exceptions;

/// <summary>
/// Thrown when a pipeline source fails to produce data (file not found,
/// connection failure, parse error, etc.). Inspect <see cref="SourceType"/>
/// and <see cref="Location"/> for structured error handling.
/// </summary>
public sealed class PipeFlowSourceException : PipeFlowException
{
    /// <summary>Kind of source that failed (e.g., "Csv", "SqlServer", "Http").</summary>
    public string SourceType { get; }

    /// <summary>Source-specific location (file path, URL, connection-string alias). May be null.</summary>
    public string? Location { get; }

    /// <summary>Row number where the failure occurred, if applicable.</summary>
    public long? RowNumber { get; }

    public PipeFlowSourceException(string sourceType, string message)
        : base(message)
    {
        SourceType = sourceType;
    }

    public PipeFlowSourceException(string sourceType, string? location, string message,
        Exception? innerException = null, long? rowNumber = null)
        : base(message, innerException)
    {
        SourceType = sourceType;
        Location = location;
        RowNumber = rowNumber;
    }
}
