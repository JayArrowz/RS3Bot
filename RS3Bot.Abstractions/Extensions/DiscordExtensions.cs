using Discord;
using RS3Bot.Abstractions.Interfaces;
using System.Linq;

namespace RS3Bot.Abstractions.Extensions
{
    public static class DiscordExtensions
    {
        public static GuildEmote GetEmote(this IDiscordBot client, string name)
        {
            var emote = client.Client.Guilds.SelectMany(x => x.Emotes).FirstOrDefault(x => x.Name.IndexOf(name) != -1);
            return emote;
        }
    }
}
