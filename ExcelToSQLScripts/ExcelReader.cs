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

        private void FillRecords(ExcelWorksheet worksheet, Table table)
        {
            for (int excelRowIndex = 2; excelRowIndex < worksheet.Dimension.Rows; excelRowIndex++)
            {
                Record record = new Record();

                foreach (var column in table.Columns)
                {
                    record[excelRowIndex-2] = new Value(column, worksheet.GetValue<string>(excelRowIndex, column.Index));
                }

                table.Records.Add(record);
            }
        }

        private void FillColumns(ExcelWorksheet worksheet, Table table)
        {
            bool over = false;
            int index = 0;

            while (!over)
            {
                string columnName = worksheet.GetValue<string>(0, index);
                string columnDataType = worksheet.GetValue<string>(1, index);

                DataType datType = (DataType)Enum.Parse(typeof(DataType), columnDataType);

                if (!string.IsNullOrEmpty(columnName))
                {
                    table.Columns.Add(new Column(columnName, datType, index));
                }
                else
                {
                    over = true;
                }

                index++;
            }
        }
    }
}
