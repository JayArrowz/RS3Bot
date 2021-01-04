using System.Linq;

namespace RS3Bot.Abstractions.Constants
{
    public class EquipmentConstants
    {
        public static readonly string[] ValidEquipmentTypes = new string[] { "Melee", "Range", "Mage", "Skilling", "Misc" };

        public static bool IsValidEquipmentType(string type)
        {
            return ValidEquipmentTypes.Any(t => t.Equals(type, System.StringComparison.InvariantCultureIgnoreCase));
        }

        /**
		 * The hat slot.
		 */
        public static readonly int Hat = 0;

        /**
		 * The cape slot.
		 */
        public static readonly int Cape = 1;

        /**
		 * The amulet slot.
		 */
        public static readonly int Amulet = 2;

        /**
		 * The weapon slot.
		 */
        public static readonly int Weapon = 3;

        /**
		 * The chest slot.
		 */
        public static readonly int Chest = 4;

        /**
		 * The shield slot.
		 */
        public static readonly int Shield = 5;

        /**
		 * The legs slot.
		 */
        public static readonly int Legs = 7;

        /**
		 * The hands slot.
		 */
        public static readonly int Hands = 9;

        /**
		 * The feet slot.
		 */
        public static readonly int Feet = 10;

        /**
		 * The ring slot.
		 */
        public static readonly int Ring = 12;

        /**
		 * The arrows slot.
		 */
        public static readonly int Arrows = 13;

    }
}
