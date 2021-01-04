using Discord;
using Discord.WebSocket;
using RS3Bot.Cli.Options;
using RS3Bot.DAL;
using RS3BotWeb.Shared;
using RS3Bot.Abstractions.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS3Bot.Abstractions.Extensions;

namespace RS3Bot.Cli.Commands
{
    public class DiceCommand : BaseCommand<DiceOption>
    {

        private IContextFactory _contextFactory;
        public DiceCommand(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, DiceOption option)
        {
            var userId = message.Author.Id.ToString();
            ulong coinAmount = 0L;
            var amountArgs = option.Amount;
            var funDice = string.IsNullOrEmpty(amountArgs);
            var random = new Random();
            var nextRoll = random.Next(1, 100);

            var eb = new EmbedBuilder { Title = "Dice Roll" };
            var diceResult = new StringBuilder($"{message.Author.Username} rolled {nextRoll} on the percentile dice, and you ");
            var win = nextRoll >= 55;
            diceResult.Append(win ? "won" : "lost");

            if (!funDice)
            {
                using (var context = _contextFactory.Create())
                {
                    var coinItem = context.UserItems.AsQueryable().FirstOrDefault(t => t.UserId == userId && t.Item.ItemId == 995);
                    coinAmount += (coinItem?.Item?.Amount ?? 0);

                    var amountNumber = StackFormatter.StackSizeToQuantity(option.Amount);
                    if(amountNumber <= 0)
                    {
                        return false;
                    }

                    if (coinAmount < (ulong)amountNumber)
                    {
                        await message.Channel.SendMessageAsync($"{message.Author} does not have {amountArgs} coins to gamble.");
                        return false;
                    }

                    if(win)
                    {
                        coinAmount += (ulong) amountNumber;
                    } else
                    {
                        coinAmount -= (ulong)amountNumber;
                    }

                    if(coinAmount == 0)
                    {
                        context.Remove(coinItem);
                    } else
                    {
                        coinItem.Item.Amount = coinAmount;
                        context.Update(coinItem);
                    }
                    await context.SaveChangesAsync();
                    diceResult.Append($" {StackFormatter.QuantityToRSStackSize(amountNumber)} GP");
                }
            }
            diceResult.Append(".");


            eb.WithDescription(diceResult.ToString());
            eb.WithThumbnailUrl("https://i.imgur.com/sySQkSX.png");
            eb.WithColor(Color.Orange);

            if (message != null)
            {
                await message.Channel.SendMessageAsync(string.Empty, embed: eb.Build());
            }
            return true;
        }
    }
}
