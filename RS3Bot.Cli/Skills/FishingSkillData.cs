using RS3Bot.Abstractions.Model;

namespace RS3Bot.Cli.Skills
{
    public class FishingSkillData
    {
        public int Level { get; set; }
        public int Xp { get; set; }
        public int Id { get; set; }
        public double TimePerFish { get; set; }
        public string Name { get; set; }
        public string[] OtherNames { get; set; }
        public Item[] ItemsRequired { get; set; }
    }
}
