using CommandLine;
using RS3Bot.Abstractions.Interfaces;

namespace RS3Bot.Cli.Options
{
    [Verb("skill", HelpText = "Start skilling")]
    public class SkillOption : IOptionsBase
    {
        [Value(0, HelpText = "The skill name (fishing, cooking, firemaking etc.)", Required = true)]
        public string SkillName { get; set; }

        [Value(1, Required = true, HelpText = "The skilling task")]
        public string Task { get; set; }

        [Value(2, Required = false, HelpText = "The Amount")]
        public int? Amount { get; set; }
    }
}
