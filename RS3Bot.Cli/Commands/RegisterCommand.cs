using Discord.WebSocket;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using RS3Bot.DAL;
using RS3Bot.Abstractions.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace RS3Bot.Cli.Commands
{
    public class RegisterCommand : BaseCommand<RegisterOption>
    {
        private readonly IContextFactory _dbContextFactory;

        public RegisterCommand(IContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, RegisterOption option)
        {
            var userId = message.Author.Id;
            using (var dbContext = _dbContextFactory.Create())
            {
                var existingUser = dbContext.Users.FirstOrDefault(t => t.DiscordId == userId);
                if (existingUser != null)
                {
                    await message.Channel.SendMessageAsync($"{message.Author.Username} already has a registered user.");
                }
                else
                {
                    var skillSet = new SkillSet();
                    skillSet.Init();
                    var applicationUser = new ApplicationUser
                    {
                        DiscordId = userId,
                        UserName = message.Author.Username,
                        NormalizedUserName = message.Author.Username.ToUpper(),
                        Id = userId.ToString(),
                        PasswordHash = "TestAtm",
                        SkillSet = skillSet,
                        CurrentTask = new CurrentTask { Notified = true, UserId = userId.ToString(), UnlockTime = DateTime.MinValue },
                        Mention = message.Author.Mention
                    };

                    dbContext.Add(applicationUser);

                    await dbContext.SaveChangesAsync();
                    await message.Channel.SendMessageAsync($"{message.Author.Username} has registered their user. Type +m for help.");
                }
            }
            return true;
        }
    }
}
