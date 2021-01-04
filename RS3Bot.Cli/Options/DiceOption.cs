using CommandLine;
using RS3Bot.Abstractions.Interfaces;

namespace RS3Bot.Cli.Options
{
    [Verb("dice", HelpText = "Roll the dice")]
    public class DiceOption : IOptionsBase
    {
        [Value(0, MetaName = "amount", Required = false, HelpText = "Amount to gamble")]
        public string Amount { get; set; }
    }
}
