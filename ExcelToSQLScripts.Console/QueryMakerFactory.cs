using System;
using ExcelToSQLScripts.QueryMakers;

namespace ExcelToSQLScripts.Console
{
    public class QueryMakerFactory
    {
        public static IQueryMaker Create(string mode, ValueRenderer valueRenderer){
            switch(mode.ToLower()){
                case "insert":
                    return new InsertQueryMaker(valueRenderer);
                case "merge":
                    return new OracleMergeQueryMaker(valueRenderer);
                default:
                    throw new ArgumentException("invalid mode specified");
            }
        }
    }
}
