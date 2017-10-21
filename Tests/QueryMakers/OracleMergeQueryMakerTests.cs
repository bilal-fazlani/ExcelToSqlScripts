using System;
using ExcelToSQLScripts;
using ExcelToSQLScripts.Models;
using ExcelToSQLScripts.QueryMakers;
using FluentAssertions;
using Xunit;
using Record = ExcelToSQLScripts.Models.Record;

namespace Tests.QueryMakers
{
    public class OracleMergeQueryMakerTests
    {
        [Fact]
        public void CanMakeQuery()
        {
            OracleMergeQueryMaker queryMaker = new OracleMergeQueryMaker(new ValueRenderer(new string[] { }));
            Record record = Utils.GetTable().Records[0];
            string query = queryMaker.GenerateQuery(record);
            query.Should().Be(@"MERGE INTO EMPLOYEES T
USING (SELECT 1 ID, 'bilal' NAME FROM DUAL) D
ON (T.ID = D.ID)
WHEN MATCHED THEN 
UPDATE SET T.NAME = D.NAME
WHEN NOT MATCHED THEN 
INSERT (ID, NAME) VALUES (D.ID, D.NAME);
");
        }
    }
}