using CommandLine;
using RS3Bot.Abstractions.Interfaces;

namespace RS3Bot.Cli.Options
{
    [Verb("shop", HelpText = "View items in shop")]
    public class ShopViewOption : IOptionsBase
    {
        [Value(1, HelpText = "The Shop name", Required = true)]
        public string Name { get; set; }

        [Option("show-id", HelpText = "Shows ID on the interface.", Required = false)]
        public bool ShowId { get; set; }
    }
}
