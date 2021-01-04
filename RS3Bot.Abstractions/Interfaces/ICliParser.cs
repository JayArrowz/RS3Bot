using Discord.WebSocket;
using System.Threading.Tasks;

namespace RS3Bot.Abstractions.Interfaces
{
    public interface ICliParser
    {
        Task<bool> ParseCommand(IDiscordBot bot, SocketMessage arg, params string[] args);
    }
}
