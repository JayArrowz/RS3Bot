using Discord;
using Microsoft.EntityFrameworkCore;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
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
        private bool _quit;
        public TaskHandler(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void Dispose()
        {
            _quit = true;
        }

        public void Start(IDiscordBot discordBot)
        {
            Thread t = new Thread(() =>
            {
                while (!_quit)
                {
                    List<CurrentTask> tasks = null;
                    using (var context = _contextFactory.Create())
                    {
                        var now = DateTime.Now;
                        tasks = context.CurrentTasks.AsQueryable()
                            .Include(t => t.User)
                            .Include(t => t.ExpGains)
                            .Where(t => !t.Notified && now > t.UnlockTime)
                            .ToList();

                        if (tasks.Any())
                        {
                            foreach (var task in tasks)
                            {
                                task.Notified = true;
                            }
                            context.UpdateRange(tasks);
                            context.SaveChanges();

                            tasks.ForEach(t =>
                            {
                                var channel = discordBot.Client.GetChannel(t.ChannelId) as IMessageChannel;
                                channel.SendMessageAsync("Finished test");
                            });
                        }

                    }
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }
            });
            t.Start();
        }

    }
}
