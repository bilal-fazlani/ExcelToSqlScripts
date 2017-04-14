using System.IO;
using ExcelToSQLScripts;
using ExcelToSQLScripts.Models;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests
{
    public class TableScriptGeneratorTests
    {
        [Fact]
        public void CanGenerateScripts()
        {
            QueryMaker queryMakerSubstitute = Substitute.For<QueryMaker>();

            queryMakerSubstitute.GenerateQuery(Arg.Any<ExcelToSQLScripts.Models.Record>()).Returns("random string\n");

            TableScriptGenerator tableScriptGenerator = new TableScriptGenerator(queryMakerSubstitute);

            Table table = Utils.GetTable(2);

            using (Script script = tableScriptGenerator.GenerateTableScript(table))
            {
                script.Name.Should().Be("Employees");
                script.Content.Should().NotBeNull();

                StreamReader streamReader = new StreamReader(script.Content);
                string content = streamReader.ReadToEnd();

                content.Should().Be("random string\nrandom string\n");
            }
        }
    }
}
