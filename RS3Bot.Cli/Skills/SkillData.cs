using RS3Bot.Abstractions.Model;

namespace RS3Bot.Cli.Skills
{
    public class SkillData
    {
        public int Level { get; set; }
        public int Xp { get; set; }
        public Item[] ItemsGained { get; set; }
        public double TimeTakenMillis { get; set; }
        public string Name { get; set; }
        public string Skill { get; set; }
        public string[] OtherNames { get; set; }
        public RequiredItem[] ItemsRequired { get; set; }
        public string Action { get; set; }
    }
}
