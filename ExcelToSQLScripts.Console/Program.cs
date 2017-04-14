using System.Collections.Generic;
using System.IO;
using ExcelToSQLScripts.Models;
using static System.Console;

namespace ExcelToSQLScripts.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = GetInput("Enter excel file path").Replace("\"","");
            string outputPath = GetInput("Enter output directory path").Replace("\"", "");

            TableScriptGenerator tableScriptGenerator = new TableScriptGenerator(new QueryMaker());

            ExcelReader excelReader = new ExcelReader();

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
        }

        private static string GetInput(string text)
        {
            WriteLine(text);
            string path = ReadLine();
            return string.IsNullOrEmpty(path) ? GetInput(text) : path;
        }
    }
}