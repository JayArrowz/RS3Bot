using Discord.WebSocket;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using RS3Bot.DAL;
using System;
using System.Threading.Tasks;

namespace RS3Bot.Cli.Commands.Impl
{
    public class StatusCommand : UserAwareCommand<StatusOption>
    {
        public StatusCommand(IContextFactory contextFactory) : base(contextFactory)
        {
        }

        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, ApplicationUser user, ApplicationDbContext context, StatusOption option)
        {
            var taskName = user.CurrentTask.TaskName;
            if (!user.CurrentTask.Notified && DateTime.Now > user.CurrentTask.UnlockTime)
            {
                await message.Channel.SendMessageAsync($"{user.Mention} is on the way back from {taskName}");
            }
            else if (user.CurrentTask.Notified || (!user.CurrentTask.Notified && DateTime.Now > user.CurrentTask.UnlockTime))
            {
                await message.Channel.SendMessageAsync($"{user.Mention} is doing nothing");
            }
            else
            {
                await message.Channel.SendMessageAsync($"{user.Mention} is currently {taskName}");
            }

            return true;
        }
    }
}
