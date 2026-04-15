namespace PipeFlow.Tests.DataRowTests;

public class ConstructionTests
{
    [Fact]
    public void DefaultConstructor_CreatesEmptyRow()
    {
        var row = new DataRow();
        row.ColumnCount.Should().Be(0);
        row.Columns.Should().BeEmpty();
    }

    [Fact]
    public void CapacityConstructor_InitialCapacityDoesNotAddColumns()
    {
        var row = new DataRow(capacity: 32);
        row.ColumnCount.Should().Be(0);
    }

    [Fact]
    public void KeyValuePairConstructor_PopulatesFromSource()
    {
        var source = new Dictionary<string, object?>
        {
            ["Name"] = "Alice",
            ["Age"] = 30,
            ["Active"] = true
        };
        var row = new DataRow(source);
        row.ColumnCount.Should().Be(3);
        row["Name"].Should().Be("Alice");
        row["Age"].Should().Be(30);
        row["Active"].Should().Be(true);
    }

    [Fact]
    public void KeyValuePairConstructor_PreservesInsertionOrder()
    {
        var source = new[]
        {
            new KeyValuePair<string, object?>("First", 1),
            new KeyValuePair<string, object?>("Second", 2),
            new KeyValuePair<string, object?>("Third", 3),
        };
        var row = new DataRow(source);
        row.Columns.Should().Equal("First", "Second", "Third");
    }
}
