using ExcelToSQLScripts;
using ExcelToSQLScripts.QueryMakers;
using FluentAssertions;
using Xunit;
using Record = ExcelToSQLScripts.Models.Record;

namespace Tests.QueryMakers
{
    public class H2MergeQueryMakerTests
    {
        [Fact]
        public void CanMakeQuery()
        {
            H2MergeQueryMaker queryMaker = new H2MergeQueryMaker(new ValueRenderer(new string[] { }));
            Record record = Utils.GetTable().Records[0];
            string query = queryMaker.GenerateQuery(record);
            query.Should().Be("MERGE INTO EMPLOYEES (ID, NAME) KEY (ID) VALUES (1, 'bilal');\n");
        }
    }
}