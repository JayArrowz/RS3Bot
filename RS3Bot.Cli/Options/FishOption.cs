using CommandLine;
using RS3Bot.Abstractions.Interfaces;

namespace RS3Bot.Cli.Options
{
    [Verb("fish", HelpText = "Catch fish (shrimp)")]
    public class FishOption : IOptionsBase
    {
        [Value(0, Required = true, HelpText = "The fish name")]
        public string FishName { get; set; }
    }
}
