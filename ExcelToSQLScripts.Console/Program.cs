using System;
using System.Collections.Generic;
using System.IO;
using ExcelToSQLScripts.Models;
using Microsoft.Extensions.CommandLineUtils;
using static System.Console;

namespace ExcelToSQLScripts.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();

            //give people help with --help
            app.HelpOption("-? | -h | --help");

            var inputArguement = app.Argument("inputFile", "Full path of excel file. File must have .xlsx extension.");
            var outputArguement = app.Argument("outputDirectory", "Path the the directory where all sql files will be stored. " +
                                                                  "If one or more files exist with same name, they will be overriden. " +
                                                                  "If output directory doesn't exist, it will be created.");

            app.OnExecute(() =>
            {
                string inputPath = inputArguement.Value ?? GetInput("Enter excel file path: ").Replace("\"", "");
                string outputPath =outputArguement.Value ?? GetInput("Enter output directory path: ").Replace("\"", "");

                TableScriptGenerator tableScriptGenerator = new TableScriptGenerator(new QueryMaker());

                ExcelReader excelReader = new ExcelReader();

                try
                {
                    IEnumerable<Table> tables = excelReader.Read(inputPath);
                    foreach (Table table in tables)
                    {
                        WriteLine($"{table.Name}.sql...");

                        using (Script script = tableScriptGenerator.GenerateTableScript(table))
                        using (FileStream fileStream = File.Create(Path.Combine(outputPath, script.Name + ".sql")))
                        {
                            script.Content.CopyTo(fileStream);
                            Write(" done");
                        }
                    }

                    return 0;
                }
                catch (FileNotFoundException ex)
                {
                    WriteLine($"file not found:  {ex.FileName}");
                    return 1;
                }
            });

            var result = app.Execute(args);
            Environment.Exit(result);
        }

        private static string GetInput(string text)
        {
            Write(text);
            string path = ReadLine();
            return string.IsNullOrEmpty(path) ? GetInput(text) : path;
        }
    }
}