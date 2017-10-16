using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelToSQLScripts.Models;
using Microsoft.Extensions.CommandLineUtils;
using static System.Console;

namespace ExcelToSQLScripts.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineApplication app = new CommandLineApplication();

            app.HelpOption("-? | -h | --help");

            CommandOption inputOption = app.Option("-i | --inputFile", "Full path of excel file. File must have .xlsx extension.", CommandOptionType.SingleValue);

            CommandOption outputOption = app.Option("-o | --outputDirectory", "Path the the directory where all sql files will be stored. " +
                                                                  "If one or more files exist with same name, they will be overriden. " +
                                                                  "If output directory doesn't exist, it will be created.", CommandOptionType.SingleValue);

            CommandOption nullRecordOption = app.Option("-e | --insertEmptyRecords",
                "Will insert NULLs in all fields for empty rows in excel sheet", CommandOptionType.NoValue);

            CommandOption workSheetsOption = app.Option("-w | --worksheet",
                "Index of worksheets to be processed. Index beings from 1. This option can be used multiple times in one command.",
                CommandOptionType.MultipleValue);

            CommandOption replacementOption = app.Option("-r | --replaceWithNULL",
                "Replace the given text with null values in script. This option can be used multiple times in one command.",
                CommandOptionType.MultipleValue);

            app.OnExecute(() =>
            {
                try
                {
                    string inputPath = inputOption.Value() ??
                                       throw new ArgumentNullException("inputFile",
                                           "Please provide a path to excel file");
                    string outputPath = outputOption.Value() ?? throw new ArgumentNullException("outputDirectory",
                                            "Please provide a directory for saving sql scripts");
                    bool readEmptyRecords = nullRecordOption.HasValue() && nullRecordOption.Value() == "on";

                    int[] worksheetsToRead = workSheetsOption.Values?.Select(int.Parse).ToArray();

                    string[] nullReplacements = replacementOption.Values?.ToArray();

                    Directory.CreateDirectory(outputPath);

                    ExcelReader excelReader = new ExcelReader(readEmptyRecords, worksheetsToRead);

                    TableScriptGenerator tableScriptGenerator = new TableScriptGenerator(new InsertQueryMaker(new ValueRenderer(nullReplacements)));

                    IEnumerable<Table> tables = excelReader.Read(inputPath);

                    foreach (Table table in tables)
                    {
                        string filePath = Path.Combine(outputPath, table.Name + ".sql");
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
                catch (ArgumentNullException ex)
                {
                    Error.WriteLine($"Error: {ex.Message}");
                    app.ShowHelp();
                    return 1;
                }
                catch (FileNotFoundException ex)
                {
                    Error.WriteLine($"file not found:  {ex.FileName}");
                    return 1;
                }
                catch (Exception ex)
                {
                    Error.WriteLine($"Error: {ex.Message}");
                    Error.WriteLine(ex.StackTrace);
                    return 1;
                }
            });

            try
            {
                int result = app.Execute(args);
                Environment.Exit(result);
            }
            catch (CommandParsingException e)
            {
                WriteLine(e.Message);
                app.ShowHelp();
                Environment.Exit(1);
            }
            
        }
    }
}