using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using RS3Bot.Abstractions.Constants;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Commands;
using RS3Bot.Cli.Items;
using RS3Bot.DAL;
using System.Linq;
using System.Threading.Tasks;

namespace RS3Bot.Cli.Commands
{
    public abstract class UserAwareCommand<TOption> : BaseCommand<TOption>
        where TOption : IOptionsBase
    {
        private readonly IContextFactory _contextFactory;
        protected bool SaveChanges { get; set; }
        protected UserAwareCommand(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        protected abstract Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, ApplicationUser user, ApplicationDbContext context, TOption option);

        protected async override Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, TOption option)
        {
            var userId = message.Author.Id.ToString();

            using (var context = _contextFactory.Create())
            {
                var user = await context.Users.AsQueryable()
                    .Include(t => t.Items)
                    .Include(t => t.Equipment)
                    .Include(t => t.CurrentTask)
                    .Include(t => t.SkillSet)
                    .ThenInclude(t => t.Skills)
                    .FirstOrDefaultAsync(t => t.Id == userId);

                if (user == null)
                {
                    await message.Channel.SendMessageAsync($"{message.Author} is not registered, please type +register.");
                    return false;
                }

                user.Bank = new Inventory(BankConstants.Size, Inventory.StackMode.STACK_ALWAYS);
                user.Bank.CopyTo(user.Items.Select(t => t.Item));
                bool res = await ExecuteCommand(bot, message, user, context, option);

                if (user.Bank.UpdateRequired)
                {
                    await BankSaver.SaveBank(context, user);
                    SaveChanges = true;
                }

                if (SaveChanges)
                {
                    await context.SaveChangesAsync();
                    SaveChanges = false;
                }

                return res;
            }
        }
    }
}
