using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.DAL;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RS3Bot.Cli
{
    public abstract class UserAwareCommand<TOption> : BaseCommand<TOption>
        where TOption : IOptionsBase
    {
        private readonly IContextFactory _contextFactory;
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
                    .FirstOrDefaultAsync(t => t.Id == userId);
                user.Bank = new Inventory(600, Inventory.StackMode.STACK_ALWAYS);
                user.Bank.CopyTo(user.Items.Select(t => t.Item));

                if (user == null)
                {
                    await message.Channel.SendMessageAsync($"{message.Author} is not registered, please type +register.");
                    return false;
                }

                var res = await ExecuteCommand(bot, message, user, context, option);
                if (user.Bank.UpdateRequired)
                {
                    context.RemoveRange(user.Items);
                    await context.SaveChangesAsync();
                    var items = user.Bank.GetItems()
                        .Where(t => t != null)
                        .Select(t => new UserItem
                        {
                            UserId = user.Id,
                            Item = t
                        }).ToList();
                    user.Bank.UpdateRequired = false;

                    context.AttachRange(items);
                    context.AddRange(items);
                    await context.SaveChangesAsync();
                }

                return res;
            }
        }
    }
}
