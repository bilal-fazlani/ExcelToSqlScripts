using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelToSQLScripts.Models;
using ExcelToSQLScripts.QueryMakers;
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

            app.Command("insert", command =>
            {
                command.Description = "Generates insert scripts";

                CommandOption inputOption = HydrateCommandWithOptions(command, out CommandOption outputOption,
                    out CommandOption nullRecordOption,
                    out CommandOption workSheetsOption, out CommandOption replacementOption);

                command.OnExecute(() => GenerateScripts(inputOption, outputOption, nullRecordOption, workSheetsOption,
                    replacementOption, command, "insert"));
            });

            app.Command("update", command =>
            {
                command.Description = "Generates update scripts";

                CommandOption inputOption = HydrateCommandWithOptions(command, out CommandOption outputOption,
                    out CommandOption nullRecordOption,
                    out CommandOption workSheetsOption, out CommandOption replacementOption);

                command.OnExecute(() => GenerateScripts(inputOption, outputOption, nullRecordOption, workSheetsOption,
                    replacementOption, command, "update"));
            });

            app.Command("merge", command =>
            {
                command.Description = "Generates merge scripts";

                CommandOption inputOption = HydrateCommandWithOptions(command, out CommandOption outputOption,
                    out CommandOption nullRecordOption,
                    out CommandOption workSheetsOption, out CommandOption replacementOption);

                command.OnExecute(() => GenerateScripts(inputOption, outputOption, nullRecordOption, workSheetsOption,
                    replacementOption, command, "merge"));
            });

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 0;
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

        private static CommandOption HydrateCommandWithOptions(CommandLineApplication command,
            out CommandOption outputOption,
            out CommandOption nullRecordOption, out CommandOption workSheetsOption, out CommandOption replacementOption)
        {
            command.HelpOption("-? | -h | --help");

            CommandOption inputOption = command.Option("-i | --inputFile",
                "Full path of excel file. File must have .xlsx extension.", CommandOptionType.SingleValue);

            outputOption = command.Option("-o | --outputDirectory",
                "Path the the directory where all sql files will be stored. " +
                "If one or more files exist with same name, they will be overriden. " +
                "If output directory doesn't exist, it will be created.", CommandOptionType.SingleValue);

            nullRecordOption = command.Option("-e | --insertEmptyRecords",
                "Will insert NULLs in all fields for empty rows in excel sheet", CommandOptionType.NoValue);

            workSheetsOption = command.Option("-w | --worksheet",
                "Index of worksheets to be processed. Index beings from 1. This option can be used multiple times in one command.",
                CommandOptionType.MultipleValue);

            replacementOption = command.Option("-r | --replaceWithNULL",
                "Replace the given text with null values in script. This option can be used multiple times in one command.",
                CommandOptionType.MultipleValue);
            return inputOption;
        }

        private static int GenerateScripts(CommandOption inputOption, CommandOption outputOption,
            CommandOption nullRecordOption, CommandOption workSheetsOption,
            CommandOption replacementOption, CommandLineApplication command, string mode)
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

                ValueRenderer valueRenderer = new ValueRenderer(nullReplacements);

                IQueryMaker queryMaker = QueryMakerFactory.Create(mode, valueRenderer);

                TableScriptGenerator tableScriptGenerator = new TableScriptGenerator(queryMaker);

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
                command.ShowHelp();
                return 1;
            }
            catch (ArgumentException ex)
            {
                Error.WriteLine($"Error: {ex.Message}");
                command.ShowHelp();
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
        }
    }
}