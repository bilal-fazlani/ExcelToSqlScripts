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

            tables.Should().HaveCount(7);
        }
    }
}
