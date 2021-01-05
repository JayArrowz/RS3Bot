using Discord.WebSocket;
using RS3Bot.Abstractions.Extensions;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using RS3Bot.DAL;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RS3Bot.Cli.Commands
{
    public class FishCommand : UserAwareCommand<FishOption>
    {
        private readonly ISimulator<FishOption> _fishingSimulator;

        public FishCommand(IContextFactory factory, ISimulator<FishOption> fishingSimulator) : base(factory)
        {
            _fishingSimulator = fishingSimulator;
        }

        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, ApplicationUser user, ApplicationDbContext context, FishOption option)
        {
            if (user.CurrentTask != null && !user.CurrentTask.Notified)
            {
                await message.Channel.SendMessageAsync("Your minion is busy!");
                return true;
            }


            var emote = bot.GetEmote("Fishing");
            var simulation = await _fishingSimulator.SimulateTask(bot, user, message, option);

            if (simulation == null)
            {
                return true;
            }

            user.CurrentTask.UnlockTime = simulation.UnlockTime;
            user.CurrentTask.TaskName = simulation.TaskName;
            user.CurrentTask.ChannelId = simulation.ChannelId;
            user.CurrentTask.Notified = false;
            user.CurrentTask.ExpGains = simulation.ExpGains;
            user.CurrentTask.CompletionMessage = simulation.CompletionMessage;
            user.CurrentTask.Items = simulation.Items;

            if (user.CurrentTask.ExpGains != null && user.CurrentTask.ExpGains.Any())
            {
                foreach (var expGains in user.CurrentTask.ExpGains)
                {
                    expGains.CurrentTaskId = user.CurrentTaskId;
                }
                context.AddRange(user.CurrentTask.ExpGains);
            }

            if (user.CurrentTask.Items != null && user.CurrentTask.Items.Any())
            {
                foreach (var item in user.CurrentTask.Items)
                {
                    item.CurrentTaskId = user.CurrentTaskId;
                }
                context.AddRange(user.CurrentTask.Items);
            }
            context.Update(user.CurrentTask);
            SaveChanges = true;
            var elapsed = user.CurrentTask.UnlockTime.Value.Subtract(DateTime.Now);
            await message.Channel.SendMessageAsync($"{user.Mention} begins to {user.CurrentTask.TaskName} for {elapsed.Hours}h {elapsed.Minutes}m {elapsed.Seconds}s!");
            return true;
        }
    }
}
