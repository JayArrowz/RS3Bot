using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS3Bot.Abstractions.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        public ulong DiscordId { get; set; }
        public string PlayerName { get; set; }
        public string Mention { get; set; }

        [Required]
        public SkillSet SkillSet { get; set; }
        public virtual ICollection<UserItem> Items { get; set; }
        public virtual ICollection<EquipmentItem> Equipment { get; set; }
        public int CurrentTaskId { get; set; }

        [Required]
        public CurrentTask CurrentTask { get; set; }

        [NotMapped]
        public Inventory Bank { get; set; }
    }
}
