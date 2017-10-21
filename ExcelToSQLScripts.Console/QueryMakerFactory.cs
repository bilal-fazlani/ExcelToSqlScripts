using System;
using ExcelToSQLScripts.QueryMakers;

namespace ExcelToSQLScripts.Console
{
    public static class QueryMakerFactory
    {
        public static IQueryMaker Create(string mode, ValueRenderer valueRenderer){
            switch(mode.ToLower()){
                case "insert":
                    return new InsertQueryMaker(valueRenderer);
                case "update":
                    return new UpdateQueryMaker(valueRenderer);
                case "merge":
                    return new OracleMergeQueryMaker(valueRenderer);
                default:
                    throw new ArgumentException("invalid mode specified");
            }
        }
    }
}
