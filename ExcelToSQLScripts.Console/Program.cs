using CommandDotNet;
using CommandDotNet.Models;

namespace ExcelToSQLScripts.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            AppRunner<App> appRunner = new AppRunner<App>(new AppSettings
            {
                Case = Case.CamelCase,
                EnableVersionOption = true,
                HelpTextStyle = HelpTextStyle.Detailed
            });

            return appRunner.Run(args);
        }
    }
}