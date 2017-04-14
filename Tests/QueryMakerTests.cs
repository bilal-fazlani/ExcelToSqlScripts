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

            Record record = Utils.GetTable().Records[0];

            string query = queryMaker.GenerateQuery(record);

            query.Should().Be("insert into Employees (ID, Name) values (1, 'bilal');\n");
        }
    }
}