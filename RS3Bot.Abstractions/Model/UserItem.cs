﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS3Bot.Abstractions.Model
{
    public class UserItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Item Item { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
