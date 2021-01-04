using System.ComponentModel.DataAnnotations.Schema;

namespace RS3Bot.Abstractions.Model
{
    public class Skill
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /**
		 * The attack id.
		 */
        public static readonly int Attack = 0;

        /**
		 * The defence id.
		 */
        public static readonly int Defence = 1;

        /**
		 * The strength id.
		 */
        public static readonly int Strength = 2;

        /**
		 * The hitpoints id.
		 */
        public static readonly int Hitpoints = 3;

        /**
		 * The ranged id.
		 */
        public static readonly int Ranged = 4;

        /**
		 * The prayer id.
		 */
        public static readonly int Prayer = 5;

        /**
		 * The magic id.
		 */
        public static readonly int Magic = 6;

        /**
		 * The cooking id.
		 */
        public static readonly int Cooking = 7;

        /**
		 * The woodcutting id.
		 */
        public static readonly int Woodcutting = 8;

        /**
		 * The fletching id.
		 */
        public static readonly int Fletching = 9;

        /**
		 * The fishing id.
		 */
        public static readonly int Fishing = 10;

        /**
		 * The firemaking id.
		 */
        public static readonly int Firemaking = 11;

        /**
		 * The crafting id.
		 */
        public static readonly int Crafting = 12;

        /**
		 * The smithing id.
		 */
        public static readonly int Smithing = 13;

        /**
		 * The mining id.rivate
		 */
        public static readonly int Mining = 14;

        /**
		 * The herblore id.
		 */
        public static readonly int Herblore = 15;

        /**
		 * The agility id.
		 */
        public static readonly int Agility = 16;

        /**
		 * The thieving id.
		 */
        public static readonly int Theiving = 17;

        /**
		 * The slayer id.
		 */
        public static readonly int Slayer = 18;

        /**
		 * The farming id.
		 */
        public static readonly int Farming = 19;

        /**
		 * The runecraft id.
		 */
        public static readonly int Runecrafting = 20;

        /**
		 * The skill names.
		 */
        private static readonly string[] SkillNames = { "Attack", "Defence", "Strength", "Constitution", "Ranged", "Prayer",
            "Magic", "Cooking", "Woodcutting", "Fletching", "Fishing", "Firemaking", "Crafting", "Smithing", "Mining",
            "Herblore", "Agility", "Thieving", "Slayer", "Farming", "Runecraft" };

        /**
         * Gets the name of a skill.
         *
         * @param id The skill's id.
         * @return The skill's name.
         */
        public static string GetName(int id)
        {
            return SkillNames[id];
        }

        /**
		 * Whether the skill affects the combat level or not.
		 *
		 * @param skill The id of the skill.
		 * @return {@code true} if the skill is a combat skill, otherwise {@code false}.
		 */
        public static bool IsCombatSkill(int skill)
        {
            return skill >= Attack && skill <= Magic;
        }

        /**
		 * Creates a skill from an existing skill, using the existing skill's experience and maximum level values, but the
		 * specified current level.
		 *
		 * @param currentLevel The current level.
		 * @param skill The existing skill.
		 *
		 * @return The new skill with the updated current level.
		 */
        public static Skill UpdateCurrentLevel(int currentLevel, Skill skill)
        {
            return new Skill(skill.SkillId, skill.Experience, currentLevel, skill.MaximumLevel);
        }

        /**
		 * Creates a skill from an existing skill, using the existing skill's current and maximum level values, but the
		 * specified experience.
		 *
		 * @param experience The experience.
		 * @param skill The existing skill.
		 * @return The new skill with the updated experience.
		 */
        public static Skill UpdateExperience(double experience, Skill skill)
        {
            return new Skill(skill.SkillId, experience, skill.CurrentLevel, skill.MaximumLevel);
        }

        /**
		 * Creates a skill from an existing skill, using the existing skill's experience and current level values, but the
		 * specified maximum level.
		 *
		 * @param maximumLevel experience The maximum level.
		 * @param skill The existing skill.
		 * @return The new skill with the updated maximum level.
		 */
        public static Skill UpdateMaximumLevel(int maximumLevel, Skill skill)
        {
            return new Skill(skill.SkillId, skill.Experience, skill.CurrentLevel, maximumLevel);
        }

        /**
		 * The current level.
		 */
        public int CurrentLevel { get; set; }

        /**
		 * The experience.
		 */
        public double Experience { get; set; }

        /**
		 * The maximum level.
		 */
        public int MaximumLevel { get; set; }

        public int SkillId { get; set; }

        /**
		 * Creates a skill.
		 *
		 * @param experience The experience.
		 * @param currentLevel The current level.
		 * @param maximumLevel The maximum level.
		 */
        public Skill(int skillId, double experience, int currentLevel, int maximumLevel)
        {
            this.SkillId = skillId;
            this.Experience = experience;
            this.CurrentLevel = currentLevel;
            this.MaximumLevel = maximumLevel;
        }
    }

}
