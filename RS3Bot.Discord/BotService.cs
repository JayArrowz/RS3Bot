using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RS3Bot.Abstractions.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace RS3BotWeb.Discord
{
    public class BotService : IHostedService
    {
        private readonly IDiscordBot _bot;
        private readonly IConfiguration _configurationRoot;
        public BotService(IDiscordBot bot, IConfiguration configurationRoot)
        {
            _configurationRoot = configurationRoot;
            _bot = bot;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _bot.LoginAsync(_configurationRoot.GetConnectionString("Discord"));

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
