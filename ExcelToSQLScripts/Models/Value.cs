namespace ExcelToSQLScripts.Models
{
    public class Value
    {
        internal readonly string StringValue;

        internal readonly Column Column;

        public Value(Column column, string stringValue)
        {
            Column = column;
            StringValue = stringValue;
        }
    }
}
