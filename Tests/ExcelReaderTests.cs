using System;
using System.Collections.Generic;
using System.Linq;
using ExcelToSQLScripts;
using ExcelToSQLScripts.Models;
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
        }
    }
}
