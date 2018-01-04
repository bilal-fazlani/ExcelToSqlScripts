﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelToSQLScripts.Models;
using OfficeOpenXml;

namespace ExcelToSQLScripts
{
    public class ExcelReader
    {
        private readonly bool _readEmptyRecords;
        private readonly int[] _worksheetsToRead;

        public ExcelReader(bool readEmptyRecords, int[] worksheetsToRead)
        {
            _readEmptyRecords = readEmptyRecords;
            _worksheetsToRead = worksheetsToRead;
        }

        public IEnumerable<Table> Read(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                ExcelPackage excel = new ExcelPackage(fileStream);

                foreach (var worksheet in excel.Workbook.Worksheets)
                {
                    if (_worksheetsToRead == null ||
                        _worksheetsToRead?.Length == 0 ||
                        (_worksheetsToRead != null && _worksheetsToRead.Contains(worksheet.Index)))
                    {
                        Table table = new Table(worksheet.Name);

                        FillColumns(worksheet, table);

                        FillRecords(worksheet, table);

                        yield return table;
                    }
                }
            }
        }

        private void FillColumns(ExcelWorksheet worksheet, Table table)
        {
            for (int i = 1; i <= worksheet?.Dimension?.Columns; i++)
            {
                string columnName = worksheet.GetValue<string>(1, i);

                if (!string.IsNullOrEmpty(columnName))
                {
                    DataType datType = GetDataType(worksheet, i);
                    table.Columns.Add(new Column(columnName, datType, i));
                }
            }
        }

        private DataType GetDataType(ExcelWorksheet worksheet, int columnIndex)
        {
            object value = worksheet.GetValue<object>(2, columnIndex);
            if (value is DateTime)
            {
                return DataType.DateTime;
            }
            
            string firstDataValue = worksheet.GetValue<string>(2, columnIndex);
            
            bool isnumber = IsNumber(firstDataValue);
            if (isnumber) return DataType.Number;

            bool isBool = IsBoolean(firstDataValue);
            if (isBool) return DataType.Boolean;
            
            return DataType.String;
        }

        private bool IsNumber(string value)
        {
            return double.TryParse(value, out double _);
        }

        private bool IsBoolean(string value)
        {
            return bool.TryParse(value, out bool _);
        }

        private void FillRecords(ExcelWorksheet worksheet, Table table)
        {
            for (int excelRowIndex = 2; excelRowIndex <= worksheet?.Dimension?.Rows; excelRowIndex++)
            {
                Record record = new Record(table);

                foreach (var column in table.Columns)
                {
                    object value = worksheet.GetValue<object>(excelRowIndex, column.Index);
                    
                    record[excelRowIndex - 2] =
                        new Value(column, worksheet.GetValue<string>(excelRowIndex, column.Index), value);
                }

                if (_readEmptyRecords || !record.IsEmpty) table.Records.Add(record);
            }
        }
    }
}