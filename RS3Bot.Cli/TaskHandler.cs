using Discord;
using Microsoft.EntityFrameworkCore;
using RS3Bot.Abstractions.Constants;
using RS3Bot.Abstractions.Extensions;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli;
using RS3Bot.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RS3Bot.Abstractions
{
    public class TaskHandler : ITaskHandler
    {
        private readonly IContextFactory _contextFactory;
        private readonly IDiscordBot _discordBot;
        private bool _quit;
        public TaskHandler(IContextFactory contextFactory, IDiscordBot discordBot)
        {
            _contextFactory = contextFactory;
            _discordBot = discordBot;
        }

        public void Dispose()
        {
            _quit = true;
        }

        public async Task Process()
        {
            while (!_quit)
            {
                List<CurrentTask> tasks = null;
                using (var context = _contextFactory.Create())
                {
                    var now = DateTime.Now;
                    tasks = context.CurrentTasks.AsQueryable()
                        .Include(t => t.ExpGains)
                        .Include(t => t.Items)
                        .Where(t => !t.Notified && now > t.UnlockTime)
                        .ToList();

                    if (tasks.Any())
                    {
                        var userIds = tasks.Select(t => t.UserId).ToList();

                        var users = context.Users.AsQueryable()
                        .Include(t => t.Items)
                        .Include(t => t.Equipment)
                        .Include(t => t.CurrentTask)
                        .Include(t => t.SkillSet)
                        .ThenInclude(t => t.Skills)
                        .Where(t => userIds.Contains(t.Id)).ToList();

                        users.ForEach(user =>
                        {
                            var bankUpdateRequired = tasks.Where(t => t.UserId == user.Id && t.Items != null && t.Items.Any()).Any();
                            if (bankUpdateRequired)
                            {
                                context.RemoveRange(user.Items);
                                user.Bank = new Inventory(BankConstants.Size, Inventory.StackMode.STACK_ALWAYS);
                                user.Bank.CopyTo(user.Items.Select(t => t.Item));
                            }
                        });

                        await context.SaveChangesAsync();

                        foreach (var task in tasks)
                        {
                            var user = users.FirstOrDefault(t => t.Id == task.UserId);
                            if (user != null)
                            {
                                if (task.Items != null && task.Items.Any())
                                {
                                    foreach (var item in task.Items)
                                    {
                                        user.Bank.Add(item.Item);
                                    }
                                    await BankSaver.SaveBank(context, user, false);
                                }
                                if (task.ExpGains.Any())
                                {
                                    foreach (var xpGain in task.ExpGains)
                                    {
                                        user.SkillSet.AddExperience(xpGain.Skill, xpGain.Amount);
                                        context.Update(user.SkillSet);
                                    }
                                }
                            }
                            var channel = _discordBot.Client.GetChannel(task.ChannelId) as IMessageChannel;
                            var xpStr = string.Join('\n', task.ExpGains.Select(xpGain => $"{_discordBot.GetEmote(Skill.GetName(xpGain.Skill))} +{xpGain.Amount} XP Gained, Level: {user.SkillSet.GetLevel(xpGain.Skill)}, Total XP: {user.SkillSet.GetExp(xpGain.Skill)}"));
                            await channel.SendMessageAsync(string.Format(task.CompletionMessage, xpStr));

                            context.RemoveRange(task.Items);
                            context.RemoveRange(task.ExpGains);
                            task.Notified = true;
                        }

                        context.UpdateRange(tasks);
                        await context.SaveChangesAsync();
                    }

                }
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        public void Start()
        {
            Task.Factory.StartNew(Process, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }
}
