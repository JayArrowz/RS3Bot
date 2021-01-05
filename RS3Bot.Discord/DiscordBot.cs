using Discord;
using Discord.WebSocket;
using RS3Bot.Abstractions.Extensions;
using RS3Bot.Abstractions.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RS3Bot.Discord
{
    public class DiscordBot : IDiscordBot
    {
        private readonly ICliParser _cliParser;

        public DiscordSocketClient Client { get; }
        public List<SocketChannel> Channels { get; set; }
        public DiscordBot(ICliParser cliParser)
        {
            _cliParser = cliParser;
            Client = new DiscordSocketClient();
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
                //split the command line input by spaces and keeping hyphens and preserve any spaces between quotes
                var splitCommandLine = Arguments.SplitCommandLine(commandInner);
                await _cliParser.ParseCommand(this, socketMessage, splitCommandLine);
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
