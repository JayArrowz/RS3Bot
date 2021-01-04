using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RS3Bot.Abstractions.Model
{
    public class SkillSet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /**
	    * The maximum allowed experience.
	    */
        public static readonly double MAXIMUM_EXP = 200_000_000;

        /**
         * The minimum amounts of experience required for the levels.
         */
        private static readonly int[] EXPERIENCE_FOR_LEVEL = new int[100];

        /**
         * The number of skills.
         */
        private static readonly int SKILL_COUNT = 21;

        static SkillSet()
        {
            int points = 0, output = 0;
            for (int level = 1; level <= 99; level++)
            {
                EXPERIENCE_FOR_LEVEL[level] = output;
                points += (int)Math.Floor(level + 300 * Math.Pow(2, level / 7.0));
                output = (int)Math.Floor(points / 4.0);
            }
        }

        public void Init()
        {
            Skills = Enumerable.Range(0, SKILL_COUNT)
                .Select(id => id == Skill.Hitpoints ? new Skill(id, 1154, 10, 10) : new Skill(id, 0, 1, 1))
                .ToList();
        }

        /**
		 * Adds experience to the specified skill.
		 *
		 * @param id The skill id.
		 * @param experience The amount of experience.
		 */
        public void AddExperience(int id, double experience)
        {
            Skill old = Skills.FirstOrDefault(t => t.SkillId == id);

            double newExperience = Math.Min(old.Experience + experience, MAXIMUM_EXP);

            int current = old.CurrentLevel;
            int maximum = GetLevelForExperience(newExperience);

            int delta = maximum - old.MaximumLevel;
            current += delta;

            SetSkill(id, new Skill(id, newExperience, current, maximum));
        }

        /**
		 * Sets a {@link Skill}.
		 *
		 * @param id The id.
		 * @param skill The skill.
		 */
        public void SetSkill(int id, Skill skill)
        {
            Skill old = Skills.FirstOrDefault(t => t.SkillId == id);
            Skills.Remove(old);
            Skills.Add(skill);
        }


        /**
		 * Gets the minimum experience required for the specified level.
		 *
		 * @param level The level.
		 * @return The minimum experience.
		 */
        public static int GetExperienceForLevel(int level)
        {
            return EXPERIENCE_FOR_LEVEL[level];
        }

        /**
		 * Gets the minimum level to get the specified experience.
		 *
		 * @param experience The experience.
		 * @return The minimum level.
		 */
        public static int GetLevelForExperience(double experience)
        {
            for (int level = 1; level <= 98; level++)
            {
                if (experience < EXPERIENCE_FOR_LEVEL[level + 1])
                {
                    return level;
                }
            }

            return 99;
        }

        /**
		 * The combat level of this skill set.
		 */
        public int Combat { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
    }
}
