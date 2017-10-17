using System;
using System.Linq;
using System.Text;
using ExcelToSQLScripts.Models;

namespace ExcelToSQLScripts.QueryMakers
{
    public class OracleMergeQueryMaker : IQueryMaker
    {
        readonly ValueRenderer _valueRenderer;

        public OracleMergeQueryMaker(ValueRenderer valueRenderer)
        {
            _valueRenderer = valueRenderer;
        }

        public string GenerateQuery(Record record)
        {
            //StringBuilder stringBuilder = new StringBuilder();
            string tableName = record.Table.Name.ToUpperInvariant();
            string primaryKeyColumnName = record.Table.PrimaryKey.Name.ToUpperInvariant();
            string columnNameValuePairs = string.Join(", ",
                                                      record.Table.Columns
                                                      .Select(c => $"{_valueRenderer.Render(record.Values.Single(v => v.Column.Name.ToUpperInvariant() == c.Name.ToUpperInvariant()))} {c.Name.ToUpperInvariant()}"));
            string columnNamesForUpdate = string.Join(", ",
                                                      record.Table.Columns.Skip(1)
                                                      .Select(c => $"T.{c.Name.ToUpperInvariant()} = D.{c.Name.ToUpperInvariant()}"));

            string allColumnNames = string.Join(", ",
                                             record.Table.Columns.Select(c=> c.Name.ToUpperInvariant()));

            string dPrefixedColumnNames = string.Join(", ",
                                                      record.Table.Columns.Select(c => $"D.{c.Name.ToUpperInvariant()}"));

             return $@"MERGE INTO {tableName} T
USING (SELECT {columnNameValuePairs} FROM DUAL) D
ON (T.{primaryKeyColumnName} = D.{primaryKeyColumnName})
WHEN MATCHED THEN 
UPDATE SET {columnNamesForUpdate}
WHEN NOT MATCHED THEN 
INSERT ({allColumnNames}) VALUES ({dPrefixedColumnNames});
";
        }
    }
}
