namespace PipeFlow.Tests.DataRowTests;

public class CloneTests
{
    [Fact]
    public void Clone_ProducesEqualButDistinctInstance()
    {
        var original = new DataRow { ["A"] = 1, ["B"] = "two" };
        var cloned = original.Clone();
        cloned.Should().NotBeSameAs(original);
        cloned.Equals(original).Should().BeTrue();
    }

    [Fact]
    public void Clone_IsIndependent_MutatingCloneDoesNotAffectOriginal()
    {
        var original = new DataRow { ["A"] = 1 };
        var cloned = original.Clone();
        cloned["A"] = 99;
        cloned["B"] = "new";
        original["A"].Should().Be(1);
        original.ContainsColumn("B").Should().BeFalse();
    }

    [Fact]
    public void Clone_PreservesColumnOrder()
    {
        var original = new DataRow { ["Z"] = 1, ["A"] = 2, ["M"] = 3 };
        var cloned = original.Clone();
        cloned.Columns.Should().Equal("Z", "A", "M");
    }
}
