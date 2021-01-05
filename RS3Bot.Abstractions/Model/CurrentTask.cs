using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS3Bot.Abstractions.Model
{
    public class CurrentTask
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string TaskName { get; set; }
        public DateTime? UnlockTime { get; set; }
        public bool Notified { get; set; }
        public ulong ChannelId { get; set; }
        public string CompletionMessage { get; set; }
        public virtual ICollection<ExpGain> ExpGains { get; set; }
        public virtual ICollection<TaskItem> Items { get; set; } 
    }
}
