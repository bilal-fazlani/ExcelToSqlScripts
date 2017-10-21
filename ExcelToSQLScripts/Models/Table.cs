using System.Collections.Generic;
using System.Linq;

namespace ExcelToSQLScripts.Models
{
    public class Table
    {
        public Table(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public List<Column> Columns { get; set; } = new List<Column>();

        public List<Record> Records { get; set; } = new List<Record>();

        public string PrimaryKeyName
        {
            get { return Columns.OrderBy(c => c.Index).FirstOrDefault().Name; }
        }
    }
}