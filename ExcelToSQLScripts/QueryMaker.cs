using System.Text;
using ExcelToSQLScripts.Models;

namespace ExcelToSQLScripts
{
    public class QueryMaker
    {
        public string GenerateQuery(Record record)
        {
            StringBuilder stringBuilder = new StringBuilder("insert into ");

            stringBuilder.Append(record.Table.Name + " (");

            foreach (var column in record.Table.Columns)
            {
                stringBuilder.Append(column.Name);
                if (column.Index != record.Table.Columns.Count)
                {
                    stringBuilder.Append(", ");
                }
            }
            stringBuilder.Append(") values (");

            int index = 0;

            foreach (var value in record.Values)
            {
                if (value.Column.DataType == DataType.String) stringBuilder.Append("'");

                stringBuilder.Append(value.StringValue);

                if (value.Column.DataType == DataType.String) stringBuilder.Append("'");

                if (index < record.Values.Count-1) stringBuilder.Append(", ");

                index++;
            }

            stringBuilder.Append(");");

            return stringBuilder.ToString();
        }
    }
}
