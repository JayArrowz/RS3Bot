using Discord.WebSocket;
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
        public FishCommand(IContextFactory factory) : base(factory)
        {
        }

        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, ApplicationUser user, ApplicationDbContext context, FishOption option)
        {
            if(user.CurrentTask != null && !user.CurrentTask.Notified)
            {
                await message.Channel.SendMessageAsync("Your minion is busy!");
                return true;
            }


            var emote = bot.Client.Guilds.SelectMany(x => x.Emotes)
                .FirstOrDefault(x => x.Name.IndexOf("Fishing") != -1);

            user.CurrentTask.UnlockTime = DateTime.Now.AddMinutes(1);
            user.CurrentTask.TaskName = "fishing shrimps";
            user.CurrentTask.ChannelId = message.Channel.Id;
            user.CurrentTask.Notified = false;
            context.Update(user.CurrentTask);
            SaveChanges = true;
            await message.Channel.SendMessageAsync($"Your minion begins to fish Shrimp for 1 minute {emote}!");             
            return true;
        }
    }
}
