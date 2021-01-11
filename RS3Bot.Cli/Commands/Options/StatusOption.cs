using CommandLine;
using RS3Bot.Abstractions.Interfaces;

namespace RS3Bot.Cli.Commands.Options
{
    [Verb("status", HelpText = "Displays current status")]
    public class StatusOption : IOptionsBase
    {
    }
}
