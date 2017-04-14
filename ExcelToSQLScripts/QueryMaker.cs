using System.Text;
using ExcelToSQLScripts.Models;

namespace ExcelToSQLScripts
{
    public class QueryMaker
    {
        public virtual string GenerateQuery(Record record)
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
                stringBuilder.Append(value.GetStringValue());

                if (index < record.Values.Count-1) stringBuilder.Append(", ");

                index++;
            }

            stringBuilder.Append(");\n");

            return stringBuilder.ToString();
        }
    }
}
