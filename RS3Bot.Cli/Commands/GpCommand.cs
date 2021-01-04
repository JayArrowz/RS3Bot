using Discord;
using Discord.WebSocket;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Cli.Options;
using RS3Bot.DAL;
using System.Threading.Tasks;

namespace RS3Bot.Cli.Commands
{
    public class GpCommand : UserAwareCommand<GpOption>
    {
        public GpCommand(IContextFactory contextFactory) : base(contextFactory) { }

        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, Abstractions.Model.ApplicationUser user, ApplicationDbContext context, GpOption option)
        {
            var coins = user.Bank.GetAmount(995);

            var eb = new EmbedBuilder { Title = "GP Count" };
            eb.WithDescription($"@{message.Author} has {coins} gp.");
            eb.WithThumbnailUrl("https://runescape.wiki/images/6/63/Coins_detail.png");
            eb.WithColor(Color.Gold);

            await message.Channel.SendMessageAsync(string.Empty, embed: eb.Build());
            return true;
        }
    }
}
