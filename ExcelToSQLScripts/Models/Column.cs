namespace ExcelToSQLScripts.Models
{
    public class Column
    {
        public Column(string name, DataType dataType, int index)
        {
            Name = name;
            DataType = dataType;
            Index = index;
        }

        public string Name { get; set; }

        public DataType DataType { get; set; }

        public int Index { get; }
    }
}
