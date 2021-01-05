using Autofac;
using Discord.WebSocket;
using RS3Bot.Abstractions.Model;
using System.Threading.Tasks;

namespace RS3Bot.Abstractions.Interfaces
{
    public interface ISimulator<TArgs> : IStartable
    {
        Task<CurrentTask> SimulateTask(IDiscordBot bot, ApplicationUser user, SocketMessage message, TArgs args);
    }
}
