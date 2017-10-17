namespace ExcelToSQLScripts.Models
{
    public class Value
    {
        public Value(Column column, string stringValue)
        {
            Column = column;
            StringValue = stringValue;
        }

        internal readonly string StringValue;

        internal readonly Column Column;
    }
}
