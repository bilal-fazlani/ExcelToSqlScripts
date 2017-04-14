using System;

namespace ExcelToSQLScripts.Models
{
    public class Value
    {
        private readonly string _value;

        public Value(Column column, string stringValue)
        {
            Column = column;
            _value = stringValue;
        }

        public Column Column { get; set; }

        public string GetStringValue()
        {
            if (string.IsNullOrEmpty(_value) || string.Equals(_value, Constants.NULL, StringComparison.OrdinalIgnoreCase)) return Constants.NULL;

            switch (Column.DataType)
            {
                case DataType.Number:
                    return _value;
                case DataType.String:
                    return $"'{_value.Replace("'", "''")}'";
                default:
                    //todo: fix this
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
