namespace PipeFlow.Tests.DataRowTests;

public class EqualityTests
{
    [Fact]
    public void Equals_IdenticalContent_ReturnsTrue()
    {
        var a = new DataRow { ["Name"] = "Alice", ["Age"] = 30 };
        var b = new DataRow { ["Name"] = "Alice", ["Age"] = 30 };
        a.Equals(b).Should().BeTrue();
        (a == b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentOrderSameContent_ReturnsTrue()
    {
        var a = new DataRow { ["Name"] = "Alice", ["Age"] = 30 };
        var b = new DataRow { ["Age"] = 30, ["Name"] = "Alice" };
        a.Equals(b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentCaseKeys_ReturnsTrue()
    {
        var a = new DataRow { ["Name"] = "Alice" };
        var b = new DataRow { ["NAME"] = "Alice" };
        a.Equals(b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var a = new DataRow { ["Name"] = "Alice" };
        var b = new DataRow { ["Name"] = "Bob" };
        a.Equals(b).Should().BeFalse();
    }

    [Fact]
    public void Equals_DifferentColumnSets_ReturnsFalse()
    {
        var a = new DataRow { ["Name"] = "Alice" };
        var b = new DataRow { ["Name"] = "Alice", ["Age"] = 30 };
        a.Equals(b).Should().BeFalse();
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        var a = new DataRow { ["X"] = 1 };
        // CA1508: intentionally testing null-branch of Equals/operator==
#pragma warning disable CA1508
        a.Equals(null).Should().BeFalse();
        (a == null).Should().BeFalse();
#pragma warning restore CA1508
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        var a = new DataRow { ["X"] = 1 };
        object other = "not a DataRow";
        a.Equals(other).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_IsCached()
    {
        var a = new DataRow { ["X"] = 1, ["Y"] = 2 };
        var first = a.GetHashCode();
        var second = a.GetHashCode();
        first.Should().Be(second);
    }

    [Fact]
    public void GetHashCode_ChangesAfterMutation()
    {
        var a = new DataRow { ["X"] = 1 };
        var before = a.GetHashCode();
        a["X"] = 2;
        var after = a.GetHashCode();
        after.Should().NotBe(before);
    }

    [Fact]
    public void HashSet_OfDataRows_DeduplicatesStructurally()
    {
        // v2 bug: Distinct broken because DataRow lacked IEquatable
        var set = new HashSet<DataRow>
        {
            new DataRow { ["N"] = 1 },
            new DataRow { ["N"] = 1 },   // duplicate
            new DataRow { ["N"] = 2 },
        };
        set.Should().HaveCount(2);
    }
}
