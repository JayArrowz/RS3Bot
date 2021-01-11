using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using RS3Bot.DAL;
using RS3Bot.Abstractions.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS3Bot.Cli.Commands.Impl
{
    public class StatsCommand : BaseCommand<StatsOptions>
    {
        private readonly IContextFactory _contextFactory;
        public StatsCommand(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, StatsOptions option)
        {
            var userId = message.Author.Id.ToString();
            var eb = new EmbedBuilder { Title = $"{message.Author} Stats         ",  Color = Color.DarkPurple };
            using (var context = _contextFactory.Create())
            {
                var skillSet = await context.SkillSets.Include(t => t.Skills)
                    .FirstOrDefaultAsync(t => t.UserId == userId);
                if(skillSet == null)
                {
                    await message.Channel.SendMessageAsync($"{message.Author} is not registered, please type +register.");
                    return false;
                }
                var orderedSkills = skillSet.Skills.OrderBy(t => t.SkillId);

                int skillAdded = 0;
                foreach (var skill in orderedSkills)
                {
                    var skillName = Skill.GetName(skill.SkillId);
                    var emote = bot.Client.Guilds.SelectMany(x => x.Emotes).FirstOrDefault(x => x.Name.IndexOf(skillName) != -1);                    
                    var otherField = new EmbedFieldBuilder().WithName(emote.ToString())
                        .WithValue(skill.MaximumLevel.ToString())
                        .WithIsInline(true);
                    eb.AddField(otherField);
                    skillAdded++;
                }
            }
            await message.Channel.SendMessageAsync(string.Empty, embed: eb.Build());
            return true;
        }
    }
}
