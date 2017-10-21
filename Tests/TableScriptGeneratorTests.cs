using System.IO;
using ExcelToSQLScripts;
using ExcelToSQLScripts.Models;
using ExcelToSQLScripts.QueryMakers;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace Tests
{
    public class TableScriptGeneratorTests
    {
        [Fact]
        public void CanGenerateScripts()
        {
            IQueryMaker queryMakerSubstitute = Substitute.For<IQueryMaker>();

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