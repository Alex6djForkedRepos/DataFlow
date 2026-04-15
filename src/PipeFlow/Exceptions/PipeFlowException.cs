// CA1032: Custom exception constructors intentionally omit the parameterless ctor
// and the standard (string, Exception) ctor because all derived types enforce
// structured context (source type, sink type, option name, or validation errors).
// Consumers must supply the required domain context rather than a raw string.
#pragma warning disable CA1032
namespace PipeFlow.Exceptions;

/// <summary>
/// Base class for all exceptions thrown by PipeFlow. Catch this type to handle
/// any error originating from the library while letting unrelated exceptions propagate.
/// </summary>
public abstract class PipeFlowException : Exception
{
    protected PipeFlowException(string message) : base(message) { }

    protected PipeFlowException(string message, Exception? innerException)
        : base(message, innerException) { }
}
