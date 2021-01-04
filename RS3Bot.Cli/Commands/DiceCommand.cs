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
using RS3Bot.Abstractions.Model;

namespace RS3Bot.Cli.Commands
{
    public class DiceCommand : UserAwareCommand<DiceOption>
    {
        public DiceCommand(IContextFactory contextFactory) : base(contextFactory) { }

        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, Abstractions.Model.ApplicationUser user, ApplicationDbContext context, DiceOption option)
        {
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
                var coinItem = user.Bank.GetAmount(995);

                var amountNumber = StackFormatter.StackSizeToQuantity(option.Amount);
                if (amountNumber <= 0)
                {
                    return false;
                }

                if (coinItem < (ulong)amountNumber)
                {
                    await message.Channel.SendMessageAsync($"{message.Author} does not have {amountArgs} coins to gamble.");
                    return false;
                }

                if (win)
                {
                    user.Bank.Add(new Item(995, amountNumber));
                }
                else
                {
                    user.Bank.Remove(new Item(995, amountNumber));
                }

                await context.SaveChangesAsync();
                diceResult.Append($" {StackFormatter.QuantityToRSStackSize((long) amountNumber)} GP");

            }
            user.Bank.Update();
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
