using Microsoft.EntityFrameworkCore;

namespace RS3Bot.Abstractions.Model
{
    [Owned]
    public class Item
    {
        public int ItemId { get; set; }
        public ulong Amount { get; set; }
    }
}
