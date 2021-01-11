using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RS3Bot.Abstractions.Interfaces;
using RS3BotWeb.Cli;
using System.Threading.Tasks;

namespace RS3Bot.Cli
{
    public class Program
    {
        public static IConfiguration GetConfiguration(string[] args = null)
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
        }

        public static async Task Main(string[] args)
        {
            var config = GetConfiguration(args);
            IServiceCollection services = new ServiceCollection();
            
            Startup startup = new Startup(config);
            startup.ConfigureServices(services);


            // Create a container-builder and register dependencies
            var builder = new ContainerBuilder();
            startup.ConfigureContainer(builder);
            builder.RegisterInstance(config).As<IConfiguration>().As<IConfigurationRoot>();

            // Populate the service-descriptors added to `IServiceCollection`
            // BEFORE you add things to Autofac so that the Autofac
            // registrations can override stuff in the `IServiceCollection`
            // as needed
            builder.Populate(services);
            using (var container = builder.Build())
            {
                using (var lifetimeScope = container.BeginLifetimeScope())
                {
                    var discordBot = container.Resolve<IDiscordBot>();
                    await discordBot.LoginAsync(config.GetConnectionString("DiscordApiKey"));
                    Serilog.Log.Logger.Information("Started RS3 Bot");
                    //TODO Better way
                    await Task.Delay(-1);
                }
            }
        }
    }
}
