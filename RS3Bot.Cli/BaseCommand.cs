using CommandLine;
using CommandLine.Text;
using Discord;
using Discord.WebSocket;
using RS3Bot.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RS3Bot.Cli
{
    public abstract class BaseCommand<TOption> : ICommand
        where TOption : IOptionsBase
    {
        public string CommandName { get; }
        public Type OptionType => typeof(TOption);
        protected BaseCommand()
        {
            CommandName = ((VerbAttribute)OptionType.GetCustomAttribute(typeof(VerbAttribute))).Name;
        }

        protected virtual Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, TOption option) { return Task.FromResult(true); }

        protected virtual Task<bool> ExecuteCommandInner(IDiscordBot bot, SocketMessage message, IOptionsBase optionsBase)
        {
            var options = optionsBase;
            return ExecuteCommand(bot, message, (TOption)options);
        }

        public async Task<bool> Parse(IDiscordBot bot, SocketMessage message, string[] args)
        {
            var parsedCommand = Parser.Default.ParseArguments(args, OptionType);
            var result = false;
            await parsedCommand
                .MapResult(async options => 
                await ExecuteCommandInner(bot, message, (IOptionsBase)options), 
                error => ErrorText(parsedCommand, message, error));
            return result;
        }

        private async Task<bool> ErrorText(ParserResult<object> parsedCommand, SocketMessage message, IEnumerable<Error> arg)
        {
            var helpText = HelpText.AutoBuild(parsedCommand, helpText => {
                helpText.Copyright = string.Empty;
                helpText.Heading = string.Empty;
                return HelpText.DefaultParsingErrorsHandler(parsedCommand, helpText);
            }, 80);
            var eb = new EmbedBuilder { Title = $"{CommandName.First().ToString().ToUpper() + string.Join("", CommandName.Skip(1))} Command Help" };
            eb.WithDescription(helpText.ToString());
            eb.WithThumbnailUrl("https://static.wikia.nocookie.net/runescape2/images/1/10/Security_book_detail.png");
            eb.WithColor(Color.Purple);
            await message.Channel.SendMessageAsync(string.Empty, embed: eb.Build());
            return true;
        }
    }
}
