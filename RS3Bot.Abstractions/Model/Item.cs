using Microsoft.EntityFrameworkCore;

namespace RS3Bot.Abstractions.Model
{
    [Owned]
    public class Item
    {
        public Item(int itemId, ulong amount)
        {
            ItemId = itemId;
            Amount = amount;
        }
        public Item()
        {

        }
        public int ItemId { get; set; }
        public ulong Amount { get; set; }
    }
}
