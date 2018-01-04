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
        private readonly string _inputFile;
        private readonly string _outputDirectory;
        private readonly List<int> _worksheetsToRead;
        private readonly bool _readEmptyRecords;
        private readonly List<string> _nullReplacements;

        public App([Option(Inherited = true, ShortName = "i", LongName = "inputFile", Description = "Full path of excel file. File must have .xlsx extension.")]string inputFile,
            [Option(Inherited = true, ShortName = "o", LongName = "outputDirectory", Description = "Path the the directory where all sql files will be stored. " +
                                                                                 "If one or more files exist with same name, they will be overriden. " +
                                                                                 "If output directory doesn't exist, it will be created.")] string outputDirectory,
            [Option(Inherited = true, ShortName = "w", LongName = "worksheet", Description = "Index of worksheets to be processed. Index beings from 1. This option can be used multiple times in one command.")] List<int> worksheetsToRead,
            [Option(Inherited = true, ShortName = "e", LongName = "insertEmptyRecords", Description = "Will insert NULLs in all fields for empty rows in excel sheet")] bool readEmptyRecords,
            [Option(Inherited = true, ShortName = "r", LongName = "replaceWithNULL", Description = "Replace the given text with null values in script. This option can be used multiple times in one command.")] List<string> nullReplacements)
        {
            _inputFile = inputFile;
            _outputDirectory = outputDirectory;
            _worksheetsToRead = worksheetsToRead;
            _readEmptyRecords = readEmptyRecords;
            _nullReplacements = nullReplacements;   
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
                if (string.IsNullOrEmpty(_inputFile))
                    throw new ArgumentNullException(nameof(_inputFile), "Please provide a path to excel file");

                if (string.IsNullOrEmpty(_outputDirectory))
                    throw new ArgumentNullException(nameof(_outputDirectory),
                        "Please provide a directory for saving sql scripts");

                Directory.CreateDirectory(_outputDirectory);

                ExcelReader excelReader = new ExcelReader(_readEmptyRecords, _worksheetsToRead?.ToArray());

                ValueRenderer valueRenderer = new ValueRenderer(_nullReplacements?.ToArray());
            
                IQueryMaker queryMaker = QueryMakerFactory.Create(mode, valueRenderer);

                TableScriptGenerator tableScriptGenerator = new TableScriptGenerator(queryMaker);

                IEnumerable<Table> tables = excelReader.Read(_inputFile);

                foreach (Table table in tables)
                {
                    string filePath = Path.Combine(_outputDirectory, table.Name + ".sql");
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
            catch (FileNotFoundException ex)
            {
                Error.WriteLine($"file not found:  {ex.FileName}");
#if DEBUG
                Error.WriteLine(ex.StackTrace);
#endif 
                return 1;
            }
            catch (Exception ex)
            {
                Error.WriteLine($"Error: {ex.Message}");
#if DEBUG
                Error.WriteLine(ex.StackTrace);
#endif 
                return 1;
            }
        }
    }
}