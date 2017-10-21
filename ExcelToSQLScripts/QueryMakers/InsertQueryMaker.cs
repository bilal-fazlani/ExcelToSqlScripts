using System.Text;
using ExcelToSQLScripts.Models;

namespace ExcelToSQLScripts.QueryMakers
{
    public class InsertQueryMaker : IQueryMaker
    {
        private readonly ValueRenderer _valueRenderer;

        public InsertQueryMaker(ValueRenderer valueRenderer)
        {
            _valueRenderer = valueRenderer;
        }

        public virtual string GenerateQuery(Record record)
        {
            StringBuilder stringBuilder = new StringBuilder("INSERT INTO ");

            stringBuilder.Append(record.Table.Name.ToUpperInvariant() + " (");

            foreach (Column column in record.Table.Columns)
            {
                stringBuilder.Append(column.Name.ToUpperInvariant());
                if (column.Index != record.Table.Columns.Count)
                {
                    stringBuilder.Append(", ");
                }
            }
            stringBuilder.Append(") VALUES (");

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