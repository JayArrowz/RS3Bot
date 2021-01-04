using CommandLine;
using RS3Bot.Abstractions.Interfaces;

namespace RS3Bot.Cli.Options
{
    [Verb("stats", HelpText = "Displays stats")]
    public class StatsOptions : IOptionsBase
    {
    }
}
