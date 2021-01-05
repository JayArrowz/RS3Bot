using CommandLine;
using RS3Bot.Abstractions.Interfaces;

namespace RS3Bot.Cli.Options
{
    [Verb("fish", HelpText = "Catch fish (shrimp)")]
    public class FishOption : IOptionsBase
    {
        [Value(2, Required = false, HelpText = "Amount of fish to catch")]
        public int? Amount { get; set; }

        [Value(1, Required = true, HelpText = "The fish name")]
        public string FishName { get; set; }
    }
}
