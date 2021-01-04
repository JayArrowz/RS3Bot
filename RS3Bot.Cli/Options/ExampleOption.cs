using CommandLine;
using RS3Bot.Abstractions.Interfaces;

namespace RS3Bot.Cli.Options
{
    [Verb("example", HelpText =
        "Request the node for the full content of a Delta, identified by its hash / address on the Dfs")]
    public class ExampleOption : IOptionsBase
    {
        [Option('h', "hash", HelpText = "The hash of the delta being requested.", Required = true)]
        public string Hash { get; set; }
    }
}
