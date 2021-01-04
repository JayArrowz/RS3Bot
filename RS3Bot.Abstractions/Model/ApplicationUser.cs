using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RS3Bot.Abstractions.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        public ulong DiscordId { get; set; }
        public string PlayerName { get; set; }

        public SkillSet SkillSet { get; set; }
        public virtual ICollection<UserItem> Items { get; set; }
    }
}
