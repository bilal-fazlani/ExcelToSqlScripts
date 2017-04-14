namespace ExcelToSQLScripts.Models
{
    public class Value
    {
        public Value(Column column, string stringValue)
        {
            Column = column;
            StringValue = stringValue;
        }

        public Column Column { get; set; }

        public string StringValue { get; set; }
    }
}
