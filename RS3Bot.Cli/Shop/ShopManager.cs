using Autofac;
using Discord.WebSocket;
using RS3Bot.Abstractions.Extensions;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS3Bot.Cli
{
    public class ShopManager : IStartable
    {
        public List<Shop> ShopData { get; set; }

        public Shop GetForName(string name)
        {
            var shop = ShopData.FirstOrDefault(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
            || t.OtherNames != null && t.OtherNames.Any(v => v.Equals(name, StringComparison.InvariantCultureIgnoreCase)));
            return shop;
        }
        
        public string GetNames()
        {
            return string.Join(", ", ShopData.Select(t => t.Name));
        }

        public async Task<bool> Buy(ApplicationUser user, SocketMessage message, ShopBuyOption option)
        {
            var shop = GetForName(option.Name);
            if (shop == null)
            {
                var allShopsStr = GetNames();
                await message.Channel.SendMessageAsync($"Cannot find shop. Available shops: {allShopsStr}");
                return false;
            }

            var amount = (ulong) option.Amount;
            if (amount <= 0)
            {
                await message.Channel.SendMessageAsync($"Invalid amount {amount}");
                return false;
            }

            var isIntegerItem = int.TryParse(option.ItemNameOrId, out var itemId);
            ShopItem shopItem = null;
            if (isIntegerItem)
            {
                shopItem = shop.Items.FirstOrDefault(t => t.Item.ItemId == itemId);
            }


            if (shopItem == null)
            {
                await message.Channel.SendMessageAsync($"{option.ItemNameOrId} not found.");
                return false;
            }

            var currencyAmount = user.Bank.GetAmount(shop.Currency);
            var maxAmountCanBuy = (ulong)Math.Floor(currencyAmount / (double)shopItem.Price);
            if (maxAmountCanBuy == 0)
            {
                await message.Channel.SendMessageAsync($"You do not have enough gold to buy this item.");
                return false;
            }

            amount = Math.Min(shopItem.Item.Amount, amount);
            amount = Math.Min(maxAmountCanBuy, amount);


            var totalPrice = amount * (ulong)shopItem.Price;
            await message.Channel.SendMessageAsync($"{user.Mention} buys {amount} x {option.ItemNameOrId} for {totalPrice} coins.");

            user.Bank.Remove(shop.Currency, totalPrice);
            user.Bank.Add(shopItem.Item.ItemId, amount);
            return true;
        }

        public void Start()
        {
            ShopData = ResourceExtensions.DeserializeResource<List<Shop>>(typeof(ShopManager), "RS3Bot.Cli.Data.shops.json");
        }
    }
}
