using ExcelToSQLScripts;
using ExcelToSQLScripts.QueryMakers;
using FluentAssertions;
using Xunit;
using Record = ExcelToSQLScripts.Models.Record;

namespace Tests.QueryMakers
{
    public class UpdateQueryMakerTests
    {
        [Fact]
        public void CanMakeQuery()
        {
            UpdateQueryMaker queryMaker = new UpdateQueryMaker(new ValueRenderer(new string[] { }));
            Record record = Utils.GetTable().Records[0];
            string query = queryMaker.GenerateQuery(record);
            query.Should().Be("UPDATE EMPLOYEES SET NAME = 'bilal' WHERE ID = 1;\n");
        }
    }
}