using Discord.WebSocket;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using RS3Bot.DAL;
using System.Threading.Tasks;

namespace RS3Bot.Cli.Commands
{
    public class ShopBuyCommand : UserAwareCommand<ShopBuyOption>
    {
        private readonly ShopManager _shopManager;

        public ShopBuyCommand(IContextFactory contextFactory, ShopManager shopManager) : base(contextFactory)
        {
            _shopManager = shopManager;
        }

        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, ApplicationUser user, ApplicationDbContext context, ShopBuyOption option)
        {
            var updated = await _shopManager.Buy(user, message, option);
            if (updated)
            {
                user.Bank.Update();
                SaveChanges = true;
            }
            return true;
        }
    }
}
