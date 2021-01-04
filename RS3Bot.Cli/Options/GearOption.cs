using CommandLine;
using RS3Bot.Abstractions.Interfaces;

namespace RS3Bot.Cli.Options
{
    [Verb("gear", HelpText = "Displays gear")]
    public class GearOption : IOptionsBase
    {
        [Value(1, Default = "Melee", Required = false, HelpText = "The equipment type")]
        public string EquipmentType { get; set; }
    }
}
