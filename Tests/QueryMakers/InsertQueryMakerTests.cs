using ExcelToSQLScripts;
using ExcelToSQLScripts.Models;
using ExcelToSQLScripts.QueryMakers;
using FluentAssertions;
using Xunit;
using Record = ExcelToSQLScripts.Models.Record;

namespace Tests.QueryMakers
{
    public class QueryMakerTests
    {
        [Fact]
        public void CanMakeQuery()
        {
            InsertQueryMaker queryMaker = new InsertQueryMaker(new ValueRenderer(new string[] { }));

            Record record = Utils.GetTable().Records[0];

            string query = queryMaker.GenerateQuery(record);

            query.Should().Be("INSERT INTO EMPLOYEES (ID, NAME) VALUES (1, 'bilal');\n");
        }

        [Fact]
        public void CanReplaceSingleQuoteWithDoubleQuote()
        {
            InsertQueryMaker queryMaker = new InsertQueryMaker(new ValueRenderer(new string[] { }));

            Record record = Utils.GetTable(name: "sky's blue").Records[0];

            string query = queryMaker.GenerateQuery(record);

            query.Should().Be("INSERT INTO EMPLOYEES (ID, NAME) VALUES (1, 'sky''s blue');\n");
        }

        [Fact]
        public void CanReplaceNullReplacementsWithNulls()
        {
            InsertQueryMaker queryMaker = new InsertQueryMaker(new ValueRenderer(new[] {"n/a"}));

            Record record = Utils.GetTable(name: "N/A").Records[0];

            string query = queryMaker.GenerateQuery(record);

            query.Should().Be("INSERT INTO EMPLOYEES (ID, NAME) VALUES (1, NULL);\n");
        }
    }
}