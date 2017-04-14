using ExcelToSQLScripts.Models;

namespace Tests
{
    public static class Utils
    {
        public static Table GetTable(int length = 1)
        {
            Table table = new Table("Employees");

            var idColumn = new Column("ID", DataType.Number, 1);
            var nameColumn = new Column("Name", DataType.String, 2);

            table.Columns.Add(idColumn);
            table.Columns.Add(nameColumn);

            for (int i = 0; i < length; i++)
            {
                Record record = new Record(table)
                {
                    [0] = new Value(idColumn, $"{i+1}"),
                    [1] = new Value(nameColumn, "bilal")
                };

                table.Records.Add(record);
            }

            return table;
        }
    }
}
