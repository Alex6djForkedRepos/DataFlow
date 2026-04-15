namespace PipeFlow.Exceptions;

/// <summary>
/// Describes a single validation failure. A row may produce multiple
/// <see cref="ValidationError"/> instances (one per rule that failed).
/// </summary>
public sealed record ValidationError(
    string ColumnName,
    string Message,
    object? AttemptedValue = null,
    long? RowNumber = null);
