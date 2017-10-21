using System.Linq;
using System.Text;
using ExcelToSQLScripts.Models;

namespace ExcelToSQLScripts.QueryMakers
{
    public class UpdateQueryMaker : IQueryMaker
    {
        private readonly ValueRenderer _valueRenderer;

        public UpdateQueryMaker(ValueRenderer valueRenderer)
        {
            _valueRenderer = valueRenderer;
        }

        public virtual string GenerateQuery(Record record)
        {
            var nameValuePair = string.Join(", ",
                record.Values.Skip(1).Select(v => $"{v.Column.Name.ToUpperInvariant()} = {_valueRenderer.Render(v)}"));
                
            StringBuilder stringBuilder = new StringBuilder($"UPDATE {record.Table.Name.ToUpperInvariant()} SET {nameValuePair} " +
                                                            $"WHERE {record.Table.PrimaryKeyName.ToUpperInvariant()} = " +
                                                            $"{_valueRenderer.Render(record.PrimaryKey.Value)};\n");

            return stringBuilder.ToString();
        }
    }
}
