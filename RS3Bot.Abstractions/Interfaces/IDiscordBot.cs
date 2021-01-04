using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RS3Bot.Abstractions.Interfaces
{
    public interface IDiscordBot : IDisposable
    {
        DiscordSocketClient Client { get; }
        List<SocketChannel> Channels { get; set; }
        Task LoginAsync(string token);
    }
}