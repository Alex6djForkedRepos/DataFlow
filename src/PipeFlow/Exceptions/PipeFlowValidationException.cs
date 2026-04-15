using System.Collections.Generic;
using System.Linq;

#pragma warning disable CA1032 // Intentional: callers must supply a ValidationError list.
namespace PipeFlow.Exceptions;

/// <summary>
/// Thrown when validation is configured to throw and one or more
/// rows fail validation. Inspect <see cref="Errors"/> for the full set.
/// </summary>
public sealed class PipeFlowValidationException : PipeFlowException
{
    public IReadOnlyList<ValidationError> Errors { get; }

    public PipeFlowValidationException(IReadOnlyList<ValidationError> errors)
        : base(BuildMessage(errors))
    {
        Errors = errors;
    }

    private static string BuildMessage(IReadOnlyList<ValidationError> errors)
    {
        if (errors.Count == 1)
            return $"Validation failed: {errors[0].ColumnName}: {errors[0].Message}";

        var first = errors.Take(3).Select(e => $"{e.ColumnName}: {e.Message}");
        var suffix = errors.Count > 3 ? $" (+ {errors.Count - 3} more)" : string.Empty;
        return $"Validation failed ({errors.Count} errors): {string.Join("; ", first)}{suffix}";
    }
}
