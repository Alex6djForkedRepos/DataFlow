namespace PipeFlow.Tests.DataRowTests;

public class IndexerTests
{
    [Fact]
    public void Indexer_Get_MissingColumn_ReturnsNull()
    {
        var row = new DataRow();
        row["NonExistent"].Should().BeNull();
    }

    [Fact]
    public void Indexer_Set_AppendsNewColumnToOrder()
    {
        var row = new DataRow();
        row["First"] = 1;
        row["Second"] = 2;
        row.Columns.Should().Equal("First", "Second");
    }

    [Fact]
    public void Indexer_Set_ExistingColumn_UpdatesValueWithoutReordering()
    {
        var row = new DataRow { ["A"] = 1, ["B"] = 2 };
        row["A"] = 10;
        row.Columns.Should().Equal("A", "B");
        row["A"].Should().Be(10);
    }

    [Fact]
    public void Indexer_IsCaseInsensitive()
    {
        var row = new DataRow { ["Name"] = "Alice" };
        row["name"].Should().Be("Alice");
        row["NAME"].Should().Be("Alice");
        row["NaMe"].Should().Be("Alice");
    }

    [Fact]
    public void IntegerIndexer_Get_ReturnsValueAtColumnIndex()
    {
        var row = new DataRow { ["A"] = 1, ["B"] = 2, ["C"] = 3 };
        row[0].Should().Be(1);
        row[1].Should().Be(2);
        row[2].Should().Be(3);
    }

    [Fact]
    public void IntegerIndexer_Get_OutOfRange_Throws()
    {
        var row = new DataRow { ["A"] = 1 };
        var act = () => _ = row[5];
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void IntegerIndexer_Set_OutOfRange_Throws()
    {
        var row = new DataRow { ["A"] = 1 };
        var act = () => row[5] = 99;
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ContainsColumn_IsCaseInsensitive()
    {
        var row = new DataRow { ["Name"] = "Alice" };
        row.ContainsColumn("name").Should().BeTrue();
        row.ContainsColumn("NAME").Should().BeTrue();
        row.ContainsColumn("Missing").Should().BeFalse();
    }

    [Fact]
    public void Remove_ExistingColumn_ReturnsTrue_AndRemovesFromOrder()
    {
        var row = new DataRow { ["A"] = 1, ["B"] = 2, ["C"] = 3 };
        var removed = row.Remove("B");
        removed.Should().BeTrue();
        row.ColumnCount.Should().Be(2);
        row.Columns.Should().Equal("A", "C");
        row.ContainsColumn("B").Should().BeFalse();
    }

    [Fact]
    public void Remove_MissingColumn_ReturnsFalse()
    {
        var row = new DataRow { ["A"] = 1 };
        row.Remove("Missing").Should().BeFalse();
    }

    [Fact]
    public void Remove_IsCaseInsensitive()
    {
        var row = new DataRow { ["Name"] = "Alice" };
        row.Remove("name").Should().BeTrue();
        row.ContainsColumn("Name").Should().BeFalse();
    }
}
