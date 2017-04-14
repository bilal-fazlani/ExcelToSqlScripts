using System.Collections.Generic;
using System.Linq;
using ExcelToSQLScripts;
using ExcelToSQLScripts.Models;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class ExcelReaderTests
    {
        [Fact]
        public void CanReadExcelFile()
        {
            ExcelReader excelReader = new ExcelReader();

            List<Table> tables = excelReader.Read("Sample.xlsx").ToList();

            tables.Should()
                .HaveCount(1)
                .And
                .Subject.First()
                .Name.Should()
                .Be("Employees");

            tables.First().Columns.Should().HaveCount(3);
            tables.First().Records.Should().HaveCount(7);
        }
    }
}
