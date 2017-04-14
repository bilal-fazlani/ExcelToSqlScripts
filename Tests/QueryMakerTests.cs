using ExcelToSQLScripts;
using ExcelToSQLScripts.Models;
using FluentAssertions;
using Xunit;
using Record = ExcelToSQLScripts.Models.Record;

namespace Tests
{
    public class QueryMakerTests
    {
        [Fact]
        public void CanMakeQuery()
        {
            QueryMaker queryMaker = new QueryMaker();

            Table table = new Table("Employees");

            var idColumn = new Column("ID", DataType.Number, 1);
            var nameColumn = new Column("Name", DataType.String, 2);

            table.Columns.Add(idColumn);
            table.Columns.Add(nameColumn);

            Record record = new Record(table)
            {
                [0] = new Value(idColumn, "1"),
                [1] = new Value(nameColumn, "bilal")
            };

            table.Records.Add(record);

            string query = queryMaker.GenerateQuery(record);

            query.Should().Be("insert into Employees (ID, Name) values (1, 'bilal');");
        }
    }
}