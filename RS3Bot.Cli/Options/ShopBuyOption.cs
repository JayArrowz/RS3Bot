using CommandLine;
using RS3Bot.Abstractions.Interfaces;

namespace RS3Bot.Cli.Options
{
    [Verb("buy", HelpText = "Buy items from shop")]
    public class ShopBuyOption : IOptionsBase
    {
        [Value(1, HelpText = "The Shop name", Required = true)]
        public string Name { get; set; }

        [Value(3, HelpText = "The amount", Required = false)]
        public int? Amount { get; set; }

        [Value(2, HelpText = "Item name or Id.", Required = true)]
        public string ItemNameOrId { get; set; }

        [Option('c', "confirm", HelpText = "Confirm buy")]
        public bool? Confirm { get; set; }
    }
}
