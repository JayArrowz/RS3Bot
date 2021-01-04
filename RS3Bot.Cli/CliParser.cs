using CommandLine;
using CommandLine.Text;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using RS3Bot.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS3Bot.Cli
{
    public class CliParser : ICliParser
    {
        private readonly IServiceProvider _serviceProvider;
        public CliParser(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<bool> ParseCommand(IDiscordBot bot, SocketMessage arg, params string[] args)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var commands = scope.ServiceProvider.GetRequiredService<IEnumerable<ICommand>>();
                var parsedCommand = commands
                    .FirstOrDefault(command =>
                    command.CommandName.Equals(args[0], StringComparison.InvariantCultureIgnoreCase));
                var commandExists = parsedCommand != null;

                if (commandExists)
                {
                    return parsedCommand.Parse(bot, arg, args);
                }
                else
                {
                    var isHelp = args[0].Equals("help", StringComparison.InvariantCultureIgnoreCase);
                    if (isHelp)
                    {
                        var helpParsed = Parser.Default.ParseArguments(args, commands.Select(t => t.OptionType).ToArray());
                        helpParsed.WithNotParsed(t =>
                            {
                                var helpText = HelpText.AutoBuild(helpParsed, helpText =>
                                {
                                    helpText.Copyright = string.Empty;
                                    helpText.Heading = string.Empty;
                                    helpText.MaximumDisplayWidth = 296;
                                    helpText.AddDashesToOption = true;
                                    helpText.AdditionalNewLineAfterOption = true;
                                    helpText.AddNewLineBetweenHelpSections = true;
                                    return HelpText.DefaultParsingErrorsHandler(helpParsed, helpText);
                                }, 80);
                                var eb = new EmbedBuilder { Title = $"Command Help" };
                                eb.WithDescription(helpText.ToString());
                                eb.WithThumbnailUrl("https://static.wikia.nocookie.net/runescape2/images/1/10/Security_book_detail.png");
                                eb.WithColor(Color.Purple);
                                arg.Channel.SendMessageAsync(string.Empty, embed: eb.Build());
                            });
                    }
                }
                return Task.FromResult(false);
            }
        }
    }
}
