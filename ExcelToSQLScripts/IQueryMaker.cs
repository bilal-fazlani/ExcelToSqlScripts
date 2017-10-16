using System;
using ExcelToSQLScripts.Models;

namespace ExcelToSQLScripts
{
    public interface IQueryMaker
    {
        string GenerateQuery(Record record);
    }
}
