using System.Collections.Generic;
using System.IO;
using CommandDotNet;
using CommandDotNet.Attributes;
using FluentValidation;
using FluentValidation.Attributes;

namespace ExcelToSQLScripts.Console
{
    [Validator(typeof(OptionsValidator))]
    public class Options : IArgumentModel
    {
        [Option(Inherited = true, ShortName = "i", LongName = "inputFile",
            Description = "Full path of excel file. File must have .xlsx extension.")]
        public string InputFile { get; set; }

        [Option(Inherited = true, ShortName = "o", LongName = "outputDirectory",
            Description = "Path the the directory where all sql files will be stored. " +
                          "If one or more files exist with same name, they will be overriden. " +
                          "If output directory doesn't exist, it will be created.")]
        public string OutputDirectory { get; set; } = "./sql-scripts/";

        [Option(Inherited = true, ShortName = "w", LongName = "worksheet", Description = "Index of worksheets to be processed. Index beings from 1. This option can be used multiple times in one command.")]
        public List<int> WorksheetsToRead { get; set; }
        
        [Option(Inherited = true, ShortName = "e", LongName = "insertEmptyRecords", Description = "Will insert NULLs in all fields for empty rows in excel sheet")]
        public bool ReadEmptyRecords { get; set; }
        
        [Option(Inherited = true, ShortName = "r", LongName = "replaceWithNULL", Description = "Replace the given text with null values in script. This option can be used multiple times in one command.")] 
        public List<string> NullReplacements { get; set; }
    }

    public class OptionsValidator : AbstractValidator<Options>
    {
        public OptionsValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            
            RuleFor(x => x.InputFile).NotEmpty().WithMessage("Please provide a path to excel file")
                .Must(File.Exists).WithMessage(o => $"file not found:  {o.InputFile}");
        }
    }
}