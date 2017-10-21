using System;
using System.Text;
using ExcelToSQLScripts.Models;

namespace ExcelToSQLScripts.QueryMakers
{
    public class H2MergeQueryMaker : IQueryMaker
    {
        readonly ValueRenderer _valueRenderer;

        public H2MergeQueryMaker(ValueRenderer valueRenderer)
        {
            _valueRenderer = valueRenderer;
        }

        public string GenerateQuery(Record record)
        {
            StringBuilder stringBuilder = new StringBuilder("MERGE INTO ");
            stringBuilder.Append(record.Table.Name.ToUpperInvariant() + " (");


            foreach (Column column in record.Table.Columns)
            {
                stringBuilder.Append(column.Name.ToUpperInvariant());
                if (column.Index != record.Table.Columns.Count)
                {
                    stringBuilder.Append(", ");
                }
            }

            string keyName = record.Table.PrimaryKeyName.ToUpperInvariant();

            stringBuilder.Append($") KEY ({keyName}) VALUES (");

            int index = 0;

            foreach (Value value in record.Values)
            {
                stringBuilder.Append(_valueRenderer.Render(value));

                if (index < record.Values.Count - 1) stringBuilder.Append(", ");

                index++;
            }

            stringBuilder.Append(");\n");

            return stringBuilder.ToString();
        }
    }
}
