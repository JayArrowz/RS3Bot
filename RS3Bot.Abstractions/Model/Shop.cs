using System.Collections.Generic;

namespace RS3Bot.Abstractions.Model
{
    public class Shop
    {
        public string Name { get; set; }
        public string[] OtherNames { get; set; }
        public int Currency { get; set; }
        public List<ShopItem> Items { get; set; }
    }
}
