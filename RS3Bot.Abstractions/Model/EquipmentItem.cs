using System.ComponentModel.DataAnnotations.Schema;

namespace RS3Bot.Abstractions.Model
{
    public class EquipmentItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EquipmentSlot { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Item Item { get; set; }
    }
}
