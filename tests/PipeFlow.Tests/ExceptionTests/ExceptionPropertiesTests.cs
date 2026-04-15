using PipeFlow.Exceptions;

namespace PipeFlow.Tests.ExceptionTests;

public class ExceptionPropertiesTests
{
    [Fact]
    public void SourceException_Full_SetsAllProperties()
    {
        var inner = new InvalidOperationException("underlying");
        var ex = new PipeFlowSourceException("Csv", "input.csv", "parse failure", inner, rowNumber: 42);

        ex.SourceType.Should().Be("Csv");
        ex.Location.Should().Be("input.csv");
        ex.RowNumber.Should().Be(42);
        ex.Message.Should().Be("parse failure");
        ex.InnerException.Should().BeSameAs(inner);
    }

    [Fact]
    public void SourceException_Short_SetsSourceTypeAndMessage()
    {
        var ex = new PipeFlowSourceException("Csv", "something went wrong");

        ex.SourceType.Should().Be("Csv");
        ex.Location.Should().BeNull();
        ex.RowNumber.Should().BeNull();
        ex.Message.Should().Be("something went wrong");
    }

    [Fact]
    public void SinkException_SetsProperties()
    {
        var ex = new PipeFlowSinkException("SqlServer", "my-db", "constraint failed");

        ex.SinkType.Should().Be("SqlServer");
        ex.Location.Should().Be("my-db");
    }

    [Fact]
    public void ConfigurationException_WithOptionName_SetsProperty()
    {
        var ex = new PipeFlowConfigurationException("Delimiter", "invalid delimiter");

        ex.OptionName.Should().Be("Delimiter");
        ex.Message.Should().Be("invalid delimiter");
    }

    [Fact]
    public void ValidationException_SingleError_HasSingleErrorMessage()
    {
        var errors = new[] { new ValidationError("Email", "is required") };
        var ex = new PipeFlowValidationException(errors);

        ex.Errors.Should().ContainSingle();
        ex.Message.Should().Contain("Email");
        ex.Message.Should().Contain("is required");
    }

    [Fact]
    public void ValidationException_ManyErrors_MessageSummarizes()
    {
        var errors = Enumerable.Range(0, 10)
            .Select(i => new ValidationError($"Col{i}", "fail"))
            .ToArray();
        var ex = new PipeFlowValidationException(errors);

        ex.Errors.Should().HaveCount(10);
        ex.Message.Should().Contain("10 errors");
        ex.Message.Should().Contain("+ 7 more");
    }
}
