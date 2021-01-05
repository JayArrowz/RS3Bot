using CommandLine;
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

            simulation.Notified = false;
            user.CurrentTask.Copy(simulation);
            user.CurrentTask.Command = Parser.Default.FormatCommandLine(option);
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
            return true;
        }
    }
}
