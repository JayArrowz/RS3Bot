using Discord.WebSocket;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using RS3Bot.DAL;
using RS3Bot.Abstractions.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RS3Bot.Cli.Widget.BankWidget;
using static RS3Bot.Cli.Widget.LootWidget;

namespace RS3Bot.Cli.Commands
{
    public class BankCommand : BaseCommand<BankOption>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IWidget<BankWidgetOptions> _bankWidget;

        public BankCommand(IContextFactory contextFactory, IWidget<BankWidgetOptions> bankWidget)
        {
            _contextFactory = contextFactory;
            _bankWidget = bankWidget;
        }


        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, BankOption option)
        {
            List<UserItem> items = new List<UserItem>();
            var userId = message.Author.Id.ToString();

            using (var ctx = _contextFactory.Create())
            {
                items = ctx.UserItems.AsQueryable().Where(t => t.UserId == userId).ToList();
            }

            using (var bankWidgetImage =
                await _bankWidget.GetWidgetAsync(
                    new BankWidgetOptions { Options = option, Title = $"Bank of {message.Author}", Items = items }))
            {
                await message.Channel.SendFileAsync(bankWidgetImage, "image.png", string.Empty);
            }
            return true;
        }
    }
}
