using Discord.WebSocket;
using Newtonsoft.Json;
using RS3Bot.Abstractions.Extensions;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RS3Bot.Cli.Skills
{
    public class FishingSimulator : ISimulator<FishOption>
    {
        public readonly static TimeSpan MaxTripLength = TimeSpan.FromMinutes(40);
        public List<FishingSkillData> FishingData { get; private set; }

        public async Task<CurrentTask> SimulateTask(IDiscordBot bot, ApplicationUser user, SocketMessage message, FishOption option)
        {
            var fish = option.FishName;
            var args = GetForName(fish);
            if (args == null)
            {
                var allFishes = string.Join(", ", FishingData.Select(t => t.Name));
                await message.Channel.SendMessageAsync($"{message.Author.Mention} Invalid fish name {fish}. All Fishes:  {allFishes}");
                return null;
            }

            var scaledTimePerFish =
                TimeSpan.FromSeconds(args.TimePerFish * (1 + (100 - user.SkillSet.GetLevel(Skill.Fishing)) / 100));
            var fishingEmote = bot.GetEmote("Fishing");
            if (user.SkillSet.GetLevel(Skill.Fishing) < args.Level)
            {
                await message.Channel.SendMessageAsync($"{message.Author.Mention} needs level {args.Level} fishing to fish {args.Name}. {fishingEmote}");
                return null;
            }

            if (!option.Amount.HasValue || option.Amount <= 0)
            {
                option.Amount = (int)Math.Floor(MaxTripLength.TotalMilliseconds / scaledTimePerFish.TotalMilliseconds);
            }

            var duration = scaledTimePerFish.Multiply(option.Amount.Value);
            if (duration > MaxTripLength)
            {
                duration = MaxTripLength;
                option.Amount = (int)Math.Floor(MaxTripLength.TotalMilliseconds / scaledTimePerFish.TotalMilliseconds);
            }

            var end = DateTime.Now.Add(duration);

            var elapsed = end.Subtract(DateTime.Now);
            var taskDesc = $"{user.Mention} begins to {fishingEmote} Fish {option.Amount} x {args.Name} for {elapsed.Hours}h {elapsed.Minutes}m {elapsed.Seconds}s!";
            await message.Channel.SendMessageAsync(taskDesc);
            return new CurrentTask
            {
                ChannelId = message.Channel.Id,
                UnlockTime = end,
                TaskName = $"{fishingEmote} Fishing {option.Amount} x {args.Name}",
                MessageId = message.Id,
                ExpGains = new List<ExpGain>() { new ExpGain { Skill = Skill.Fishing, Amount = option.Amount.Value * args.Xp } },
                Items = new List<TaskItem>() { new TaskItem { Item = new Item { ItemId = args.Id, Amount = (ulong)option.Amount } } },
                CompletionMessage = $"{user.Mention} has finished fishing {option.Amount} x {args.Name}, you also received: \n {{0}}"
            };
        }

        public FishingSkillData GetForName(string name)
        {
            return FishingData.FirstOrDefault(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
            || t.OtherNames != null && t.OtherNames.Any(other => other.Equals(name, StringComparison.InvariantCultureIgnoreCase)));
        }

        public void Start()
        {
            var serializer = new JsonSerializer();

            using (var fishingStream = ResourceExtensions.GetStreamCopy(typeof(FishingSimulator), "RS3Bot.Cli.Skills.Data.fishing.json"))
            using (var jsonStream = new StreamReader(fishingStream))
            using (var jsonTextReader = new JsonTextReader(jsonStream))
            {
                FishingData = serializer.Deserialize<List<FishingSkillData>>(jsonTextReader);
            }
        }
    }
}
