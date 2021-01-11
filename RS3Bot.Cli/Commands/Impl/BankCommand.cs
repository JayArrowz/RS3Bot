using Discord.WebSocket;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using RS3Bot.DAL;
using System.Linq;
using System.Threading.Tasks;
using static RS3Bot.Cli.Widget.BankWidget;
using static RS3Bot.Cli.Widget.ShopWidget;

namespace RS3Bot.Cli.Commands.Impl
{
    public class BankCommand : UserAwareCommand<BankOption>
    {
        private readonly IWidget<BankWidgetOptions> _bankWidget;

        public BankCommand(IContextFactory contextFactory, IWidget<BankWidgetOptions> bankWidget) : base(contextFactory)
        {
            _bankWidget = bankWidget;
        }

        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, ApplicationUser user, ApplicationDbContext context, BankOption option)
        {
            using (var bankWidgetImage =
                await _bankWidget.GetWidgetAsync(
                    new BankWidgetOptions { Options = option, Title = $"Bank of {message.Author}", Items = user.Items.ToList() }))
            {
                await message.Channel.SendFileAsync(bankWidgetImage, "image.png", string.Empty);
            }
            return true;
        }
    }
}
