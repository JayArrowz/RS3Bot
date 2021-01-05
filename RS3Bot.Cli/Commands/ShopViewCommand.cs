using Discord.WebSocket;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using RS3Bot.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RS3Bot.Cli.Widget.ShopWidget;

namespace RS3Bot.Cli.Commands
{
    public class ShopViewCommand : UserAwareCommand<ShopViewOption>
    {
        private readonly ShopManager _shopManager;
        private readonly IWidget<ShopWidgetOptions> _shopWidget;

        public ShopViewCommand(IContextFactory contextFactory, ShopManager shopManager, IWidget<ShopWidgetOptions> shopWidget) : base(contextFactory)
        {
            _shopManager = shopManager;
            _shopWidget = shopWidget;
        }

        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, ApplicationUser user, ApplicationDbContext context, ShopViewOption option)
        {
            var shop = _shopManager.GetForName(option.Name);
            if (shop == null)
            {
                await message.Channel.SendMessageAsync($"Invalid shop name. Available shops: {_shopManager.GetNames()}");
                return false;
            }

            using (var shopWidget =
         await _shopWidget.GetWidgetAsync(
             new ShopWidgetOptions { Title = shop.Name, Items = shop.Items.Select(t => t.Item).ToList() }))
            {
                await message.Channel.SendFileAsync(shopWidget, "image.png", string.Empty);
            }
            return true;
        }
    }
}
