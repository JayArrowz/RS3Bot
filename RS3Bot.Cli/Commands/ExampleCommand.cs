using Discord;
using Discord.WebSocket;
using RS3Bot.Cli.Options;
using RS3Bot.Abstractions.Interfaces;
using System.Threading.Tasks;

namespace RS3Bot.Cli.Commands
{
    public class ExampleCommand : BaseCommand<ExampleOption>
    {
        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, ExampleOption option)
        {
            if(message != null)
            {
                await message.Channel.SendMessageAsync(option.Hash);
            }
            return true;
        }
    }
}
