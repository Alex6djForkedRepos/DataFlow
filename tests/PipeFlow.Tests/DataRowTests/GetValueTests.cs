using System.Globalization;

namespace PipeFlow.Tests.DataRowTests;

public class GetValueTests
{
    [Fact]
    public void GetValue_DirectMatch_ReturnsTypedValue()
    {
        var row = new DataRow { ["Count"] = 42 };
        row.GetValue<int>("Count").Should().Be(42);
    }

    [Fact]
    public void GetValue_StringToInt_ConvertsUsingInvariantCulture()
    {
        var row = new DataRow { ["N"] = "1234" };
        row.GetValue<int>("N").Should().Be(1234);
    }

    [Fact]
    public void GetValue_DecimalFormat_UsesInvariantCulture()
    {
        var row = new DataRow { ["Price"] = "19.99" };
        row.GetValue<decimal>("Price").Should().Be(19.99m);
    }

    [Fact]
    public void GetValue_MissingColumn_ReturnsDefault()
    {
        var row = new DataRow();
        row.GetValue<int>("NonExistent").Should().Be(0);
        row.GetValue<string?>("NonExistent").Should().BeNull();
    }

    [Fact]
    public void GetValue_NullValue_ReturnsDefault()
    {
        var row = new DataRow { ["X"] = null };
        row.GetValue<int>("X").Should().Be(0);
        row.GetValue<string?>("X").Should().BeNull();
    }

    [Fact]
    public void TryGetValue_ValidConversion_ReturnsTrueAndValue()
    {
        var row = new DataRow { ["Age"] = "30" };
        row.TryGetValue<int>("Age", out var age).Should().BeTrue();
        age.Should().Be(30);
    }

    [Fact]
    public void TryGetValue_InvalidConversion_ReturnsFalse()
    {
        var row = new DataRow { ["Age"] = "not a number" };
        row.TryGetValue<int>("Age", out var age).Should().BeFalse();
        age.Should().Be(0);
    }

    [Fact]
    public void TryGetValue_MissingColumn_ReturnsFalse()
    {
        var row = new DataRow();
        row.TryGetValue<int>("Missing", out var _).Should().BeFalse();
    }

    [Fact]
    public void GetValue_NullableInt_MissingReturnsNull()
    {
        // Regression: Convert.ChangeType doesn't handle Nullable<> directly;
        // implementation must unwrap via Nullable.GetUnderlyingType.
        var row = new DataRow();
        row.GetValue<int?>("Missing").Should().BeNull();
    }

    [Fact]
    public void GetValue_NullableInt_ConvertsString()
    {
        var row = new DataRow { ["Age"] = "42" };
        row.GetValue<int?>("Age").Should().Be(42);
    }

    [Fact]
    public void GetValue_NullableDateTime_ConvertsIsoString()
    {
        var row = new DataRow { ["When"] = "2026-01-15" };
        var when = row.GetValue<DateTime?>("When");
        when.Should().Be(new DateTime(2026, 1, 15));
    }

    [Fact]
    public void TryGetValue_NullableInt_SucceedsOnStringValue()
    {
        var row = new DataRow { ["N"] = "100" };
        row.TryGetValue<int?>("N", out var n).Should().BeTrue();
        n.Should().Be(100);
    }
}
