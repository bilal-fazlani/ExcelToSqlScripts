namespace ExcelToSQLScripts.Models
{
    public class Value
    {
        public Value(Column column, string stringValue, object typedValue)
        {
            Column = column;
            StringValue = stringValue;
            TypedValue = typedValue;
        }

        internal readonly string StringValue;

        internal readonly Column Column;

        internal readonly object TypedValue;
    }
}