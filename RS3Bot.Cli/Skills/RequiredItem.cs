using RS3Bot.Abstractions.Model;

namespace RS3Bot.Cli.Skills
{
    public class RequiredItem
    {
        public Item Item { get; set; }
        public bool Delete { get; set; }
    }
}