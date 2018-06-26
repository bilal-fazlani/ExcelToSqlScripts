using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandDotNet.Attributes;
using ExcelToSQLScripts.Models;
using ExcelToSQLScripts.QueryMakers;

using static System.Console;

namespace ExcelToSQLScripts.Console
{
    public class App
    {
        private readonly Options _options;

        public App(Options options)
        {
            _options = options;
        }
        
        [ApplicationMetadata(Description = "Generates insert scripts")]
        public int Insert()
        {
            return Process("insert");
        }

        [ApplicationMetadata(Description = "Generates update scripts")]
        public int Update()
        {
            return Process("update");
        }

        [ApplicationMetadata(Description = "Generates merge scripts")]
        public int Merge()
        {
            return Process("merge");
        }

        private int Process(string mode)
        {
            try
            {
                Directory.CreateDirectory(_options.OutputDirectory);

                ExcelReader excelReader = new ExcelReader(_options.ReadEmptyRecords, _options.WorksheetsToRead?.ToArray());

                ValueRenderer valueRenderer = new ValueRenderer(_options.NullReplacements?.ToArray());
            
                IQueryMaker queryMaker = QueryMakerFactory.Create(mode, valueRenderer);

                TableScriptGenerator tableScriptGenerator = new TableScriptGenerator(queryMaker);

                IEnumerable<Table> tables = excelReader.Read(_options.InputFile);

                foreach (Table table in tables)
                {
                    string filePath = Path.Combine(_options.OutputDirectory, table.Name + ".sql");
                    Write($"writing {filePath} ...");

                    if (table.Records.Any())
                    {
                        using (Script script = tableScriptGenerator.GenerateTableScript(table))
                        {
                            using (FileStream fileStream = File.Create(filePath))
                            {
                                script.Content.CopyTo(fileStream);
                                WriteLine(" done");
                            }
                        }
                    }
                    else
                    {
                        WriteLine(" empty (skipped)");
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Error.WriteLine($"Error: {ex.GetType().Name}");
                Error.WriteLine($"Error: {ex.Message}");
#if DEBUG
                Error.WriteLine(ex.StackTrace);
#endif 
                return 1;
            }
        }
    }
}