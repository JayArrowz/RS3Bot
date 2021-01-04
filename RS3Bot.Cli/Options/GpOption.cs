using CommandLine;
using RS3Bot.Abstractions.Interfaces;

namespace RS3Bot.Cli.Options
{
    [Verb("gp", HelpText = "Displays the amount of GP you have")]
    public class GpOption : IOptionsBase
    {
    }
}
