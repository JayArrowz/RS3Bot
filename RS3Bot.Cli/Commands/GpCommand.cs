using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using RS3Bot.Cli.Options;
using RS3Bot.DAL;
using RS3Bot.Abstractions.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace RS3Bot.Cli.Commands
{
    public class GpCommand : BaseCommand<GpOption>
    {
        private readonly IContextFactory _contextFactory;

        public GpCommand(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, GpOption option)
        {
            var userId = message.Author.Id.ToString();
            ulong amount = 0L;
            using (var context = _contextFactory.Create())
            {
                var coins = await context.UserItems
                    .AsQueryable()
                    .FirstOrDefaultAsync(t => t.UserId == userId && t.ItemId == 995);
                amount += (coins?.Amount ?? 0);
            }

            var eb = new EmbedBuilder { Title = "GP Count" };
            eb.WithDescription($"@{message.Author} has {amount} gp.");
            eb.WithThumbnailUrl("https://runescape.wiki/images/6/63/Coins_detail.png");
            eb.WithColor(Color.Gold);

            await message.Channel.SendMessageAsync(string.Empty, embed: eb.Build());
            return true;
        }
    }
}
