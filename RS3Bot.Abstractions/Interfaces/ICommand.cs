using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace RS3Bot.Abstractions.Interfaces
{
    public interface ICommand
    {
        /// <summary>Parses the specified arguments.</summary>
        /// <param name="args">The arguments.</param>
        Task<bool> Parse(IDiscordBot bot, SocketMessage message, string[] args);

        /// <summary>Gets the name of the command.</summary>
        /// <value>The name of the command.</value>
        string CommandName { get; }

        /// <summary>Gets the type of the option.</summary>
        /// <value>The type of the option.</value>
        Type OptionType { get; }

    }
}
