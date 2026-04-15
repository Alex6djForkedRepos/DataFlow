// Explicit import - PipeFlow.Exceptions is not in GlobalUsings because
// the exception types don't exist until Tasks 16-20 implement them.
using PipeFlow.Exceptions;

namespace PipeFlow.Tests.ExceptionTests;

public class ExceptionHierarchyTests
{
    [Fact]
    public void PipeFlowException_IsAbstract()
    {
        typeof(PipeFlowException).IsAbstract.Should().BeTrue();
    }

    [Fact]
    public void PipeFlowException_InheritsFromException()
    {
        typeof(PipeFlowException).BaseType.Should().Be(typeof(Exception));
    }

    [Fact]
    public void PipeFlowSourceException_InheritsFromPipeFlowException()
    {
        typeof(PipeFlowSourceException).BaseType.Should().Be(typeof(PipeFlowException));
    }

    [Fact]
    public void PipeFlowSinkException_InheritsFromPipeFlowException()
    {
        typeof(PipeFlowSinkException).BaseType.Should().Be(typeof(PipeFlowException));
    }

    [Fact]
    public void PipeFlowConfigurationException_InheritsFromPipeFlowException()
    {
        typeof(PipeFlowConfigurationException).BaseType.Should().Be(typeof(PipeFlowException));
    }

    [Fact]
    public void PipeFlowValidationException_InheritsFromPipeFlowException()
    {
        typeof(PipeFlowValidationException).BaseType.Should().Be(typeof(PipeFlowException));
    }

    [Fact]
    public void AllDerivedExceptions_AreSealed()
    {
        typeof(PipeFlowSourceException).IsSealed.Should().BeTrue();
        typeof(PipeFlowSinkException).IsSealed.Should().BeTrue();
        typeof(PipeFlowConfigurationException).IsSealed.Should().BeTrue();
        typeof(PipeFlowValidationException).IsSealed.Should().BeTrue();
    }
}
