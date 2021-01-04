using CommandLine;
using RS3Bot.Abstractions.Interfaces;

namespace RS3Bot.Cli.Options
{
    [Verb("bank", HelpText = "Displays your bank")]
    public class BankOption : IOptionsBase
    {
        [Option("show-id", HelpText = "Shows ID on the interface.", Required = false, Default = false)]
        public bool ShowId { get; set; }
    }
}
