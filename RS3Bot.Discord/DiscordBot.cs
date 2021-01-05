using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using RS3Bot.Abstractions.Extensions;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RS3Bot.Discord
{
    public class DiscordBot : IDiscordBot
    {
        private readonly ICliParser _cliParser;
        private readonly IContextFactory _contextFactory;
        private readonly IReplyAwaiter _replyAwaiter;

        public DiscordSocketClient Client { get; }
        public List<SocketChannel> Channels { get; set; }
        public DiscordBot(ICliParser cliParser, IContextFactory contextFactory, IReplyAwaiter replyAwaiter)
        {
            _cliParser = cliParser;
            _contextFactory = contextFactory;
            Client = new DiscordSocketClient();
            _replyAwaiter = replyAwaiter;
            Client.MessageReceived += Client_MessageReceived;
            Client.ChannelCreated += Client_ChannelCreated;
            Client.ChannelDestroyed += Client_ChannelDestroyed;
        }

        private async Task Client_MessageReceived(SocketMessage socketMessage)
        {
            var command = socketMessage.Content;
            if (!string.IsNullOrEmpty(command) && command.StartsWith("+"))
            {
                var commandInner = command.Substring(1, command.Length - 1)?.Trim();

                using (var context = _contextFactory.Create())
                {
                    var userId = socketMessage.Author.Id.ToString();
                    var user = await context.Users.AsQueryable()
                        .Include(t => t.CurrentTask)
                        .FirstOrDefaultAsync(t => t.Id == userId);
                    if (user != null)
                    {
                        var replyAwait = await _replyAwaiter.IncomingReply(user, commandInner);
                        if (replyAwait != null)
                        {
                            commandInner = replyAwait.Task.Command;
                        }
                    }
                }

                //split the command line input by spaces and keeping hyphens and preserve any spaces between quotes
                var splitCommandLine = Arguments.SplitCommandLine(commandInner);
                try
                {
                    await _cliParser.ParseCommand(this, socketMessage, splitCommandLine);
                } catch(Exception e) {

                }
            }
        }

        private Task Client_ChannelDestroyed(SocketChannel arg)
        {
            Channels.Add(arg);
            return Task.CompletedTask;
        }

        private Task Client_ChannelCreated(SocketChannel arg)
        {
            Channels.Remove(arg);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Client.Dispose();
        }

        public async Task LoginAsync(string token)
        {
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();
        }
    }
}
