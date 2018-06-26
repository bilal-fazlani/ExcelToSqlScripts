using System;
using System.Linq;
using ExcelToSQLScripts.Models;

namespace ExcelToSQLScripts
{
    public class ValueRenderer
    {
        private readonly string[] _nullReplacements;

        public ValueRenderer(string[] nullReplacements)
        {
            _nullReplacements = nullReplacements ?? new string[0];;
        }

        public string Render(Value value)
        {
            if (string.IsNullOrEmpty(value.StringValue)
                || string.Equals(value.StringValue, Constants.NULL, StringComparison.OrdinalIgnoreCase)
                || _nullReplacements.Any(x => x.Equals(value.StringValue, StringComparison.OrdinalIgnoreCase)))
                return Constants.NULL;
            
            switch (value.Column.DataType)
            {
                case DataType.DateTime:
                    try
                    {
                        DateTime dateValue = (DateTime) value.TypedValue;
                        return $"TO_DATE('{dateValue.Year:0000}/{dateValue.Month:00}/{dateValue.Day:00} " +
                               $"{dateValue.Hour:00}:{dateValue.Minute:00}:{dateValue.Second:00}', 'yyyy/mm/dd hh24:mi:ss')";
                    }
                    catch (InvalidCastException e)
                    {
                        throw new Exception($"{value.StringValue} is not in a valid date formet", e);
                    }
                case DataType.Boolean:
                    return value.StringValue.ToLower();
                case DataType.Number:
                    return value.StringValue;
                case DataType.String:
                    return $"'{value.StringValue.Replace("'", "''")}'";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}