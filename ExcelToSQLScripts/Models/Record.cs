using System.Collections.Generic;
using System.Linq;

namespace ExcelToSQLScripts.Models
{
    public class Record
    {
        public List<Value> Values { get; set; } = new List<Value>();

        public Value this[string columnName]
        {
            get { return Values.SingleOrDefault(x => x.Column.Name == columnName); }
        }

        public Value this[int columnIndex]
        {
            get => Values[columnIndex];
            set => Values.Add(value);
        }
    }
}
