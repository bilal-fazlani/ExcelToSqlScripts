using System.Collections.Generic;
using System.Linq;

namespace ExcelToSQLScripts.Models
{
    public class Record
    {
        public Table Table { get; }

        public Record(Table table)
        {
            Table = table;
        }

        public List<Value> Values { get; set; } = new List<Value>();

        public Value this[int columnIndex]
        {
            get => Values[columnIndex];
            set => Values.Add(value);
        }

        public bool IsEmpty => Values.TrueForAll(x => string.IsNullOrEmpty(x.StringValue));
        
        public (string Name, Value Value) PrimaryKey{
            get
            {
                string name = Table.Columns.OrderBy(c => c.Index).First().Name;
                Value value = Values.Single(x => x.Column.Name.ToUpperInvariant() == name);
                return (name, value);
            }
        }
    }
}
