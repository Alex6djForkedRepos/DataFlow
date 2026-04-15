#pragma warning disable CA1032 // Intentional: callers must supply OptionName context.
namespace PipeFlow.Exceptions;

/// <summary>
/// Thrown when PipeFlow detects invalid configuration: bad option values, unresolved
/// services, unsafe identifiers, etc.
/// </summary>
public sealed class PipeFlowConfigurationException : PipeFlowException
{
    /// <summary>Name of the option or configuration key that triggered the failure, if known.</summary>
    public string? OptionName { get; }

    public PipeFlowConfigurationException(string message) : base(message) { }

    public PipeFlowConfigurationException(string? optionName, string message,
        Exception? innerException = null)
        : base(message, innerException)
    {
        OptionName = optionName;
    }
}
