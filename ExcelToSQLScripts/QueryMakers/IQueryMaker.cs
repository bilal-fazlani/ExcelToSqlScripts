using System;
using ExcelToSQLScripts.Models;

namespace ExcelToSQLScripts.QueryMakers
{
    public interface IQueryMaker
    {
        string GenerateQuery(Record record);
    }
}
