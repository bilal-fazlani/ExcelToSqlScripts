using System;
using System.Collections.Generic;
using System.IO;
using ExcelToSQLScripts.Models;
using OfficeOpenXml;

namespace ExcelToSQLScripts
{
    public class ExcelReader
    {
        public IEnumerable<Table> Read(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                ExcelPackage excel = new ExcelPackage(fileStream);

                foreach(var worksheet in excel.Workbook.Worksheets)
                {
                    Table table = new Table(worksheet.Name);

                    FillColumns(worksheet, table);

                    FillRecords(worksheet, table);

                    yield return table;
                }
            }
        }

        private void FillColumns(ExcelWorksheet worksheet, Table table)
        {
            for (int i = 1; i <= worksheet.Dimension.Columns; i++)
            {
                string columnName = worksheet.GetValue<string>(1, i);
                string columnDataType = worksheet.GetValue<string>(2, i);

                if (!string.IsNullOrEmpty(columnName))
                {
                    DataType datType = (DataType)Enum.Parse(typeof(DataType), columnDataType);
                    table.Columns.Add(new Column(columnName, datType, i));
                }
            }
        }

        private void FillRecords(ExcelWorksheet worksheet, Table table)
        {
            for (int excelRowIndex = 3; excelRowIndex <= worksheet.Dimension.Rows; excelRowIndex++)
            {
                Record record = new Record(table);

                foreach (var column in table.Columns)
                {
                    record[excelRowIndex - 3] = new Value(column, worksheet.GetValue<string>(excelRowIndex, column.Index));
                }

                if (!record.Values.TrueForAll(x => x.GetStringValue() == Constants.NULL))
                {
                    table.Records.Add(record);
                }
            }
        }
    }
}
