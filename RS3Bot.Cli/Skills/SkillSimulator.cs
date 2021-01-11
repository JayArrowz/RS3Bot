using Discord.WebSocket;
using RS3Bot.Abstractions.Extensions;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS3Bot.Cli.Skills
{
    public class SkillSimulator : ISimulator<SkillOption>
    {
        public readonly static TimeSpan MaxTripLength = TimeSpan.FromMinutes(40);
        public List<SkillData> SkillingData { get; private set; }

        public async Task<CurrentTask> SimulateTask(IDiscordBot bot, ApplicationUser user, SocketMessage message, SkillOption option)
        {
            var skill = Skill.IsSkill(option.SkillName);
            var skillId = Skill.GetId(option.SkillName);
            if (!skill)
            {
                await message.Channel.SendMessageAsync($"{message.Author.Mention} skill not found. Available skills {Skill.GetNames()}");
                return null;
            }

            option.SkillName = Skill.GetName(skillId);
            var emote = bot.GetEmote(option.SkillName);

            var skillTask = option.Task;
            var skillData = GetForName(skillTask, option.SkillName);
            if (skillData == null)
            {
                var allSkillItems = string.Join(", ", SkillingData.Where(t => t.Skill.Equals(option.SkillName, StringComparison.InvariantCultureIgnoreCase)).Select(t => t.Name));
                await message.Channel.SendMessageAsync($"{message.Author.Mention} Invalid {emote} {option.SkillName} item name {skillTask}. All {option.SkillName} items:  {allSkillItems}");
                return null;
            }

            var scaledTime =
                TimeSpan.FromMilliseconds(skillData.TimeTakenMillis * (1 + (100 - user.SkillSet.GetLevel(skillId)) / 100));

            if (user.SkillSet.GetLevel(skillId) < skillData.Level)
            {
                await message.Channel.SendMessageAsync($"{message.Author.Mention} needs level {skillData.Level} {skillData.Skill} to {skillData.Action.ToLower()} {skillData.Name}. {emote}");
                return null;
            }

            if (!option.Amount.HasValue || option.Amount <= 0)
            {
                option.Amount = (int)Math.Floor(MaxTripLength.Divide(scaledTime));
            }

            var duration = scaledTime.Multiply(option.Amount.Value);
            if (duration > MaxTripLength)
            {
                duration = MaxTripLength;
                option.Amount = (int)Math.Floor(MaxTripLength.TotalMilliseconds / scaledTime.TotalMilliseconds);
            }

            var end = DateTime.Now.Add(duration);

            var elapsed = end.Subtract(DateTime.Now);


            var requiredItems = skillData.ItemsRequired == null ? string.Empty : string.Join(", ",
                skillData.ItemsRequired.Where(item => !user.Bank.Contains(item.Item.ItemId) || user.Bank.GetAmount(item.Item.ItemId) < (item.Delete ? (ulong)option.Amount : 1))
                .Select(item => $"{item.Item.Amount * (item.Delete ? (ulong)option.Amount : 1)} x {ItemDefinition.GetItemName(item.Item.ItemId)}"));

            if (!string.IsNullOrEmpty(requiredItems))
            {
                await message.Channel.SendMessageAsync($"{message.Author.Mention} needs {requiredItems} to {skillData.Action.ToLower()} {skillData.Name}. {emote}");
                return null;
            }


            if (skillData.ItemsRequired != null && skillData.ItemsRequired.Any())
            {
                skillData.ItemsRequired.Where(t => t.Delete)
                    .ToList()
                    .ForEach(item =>
                    {
                        user.Bank.RemoveItem(new Item(item.Item.ItemId, (ulong)option.Amount));
                        user.Bank.UpdateRequired = true;
                    });
            }
            var taskDesc = $"{user.Mention} begins to {emote} {skillData.Action.ToLower()} {option.Amount} x {skillData.Name} for {elapsed.Hours}h {elapsed.Minutes}m {elapsed.Seconds}s!";
            await message.Channel.SendMessageAsync(taskDesc);
            var task = new CurrentTask
            {
                ChannelId = message.Channel.Id,
                UnlockTime = end,
                TaskName = $"{emote} {option.SkillName} {option.Amount} x {skillData.Name}",
                MessageId = message.Id,
                ExpGains = new List<ExpGain>() { new ExpGain { Skill = skillId, Amount = option.Amount.Value * skillData.Xp } },
                Items = skillData.ItemsGained?.Select(t => new TaskItem { Item = new Item { ItemId = t.ItemId, Amount = t.Amount * (ulong)option.Amount } }).ToList(),
                CompletionMessage = $"{user.Mention} has finished the task and retrieved {{1}}, you also received: \n {{0}}"
            };

            var retriveStr = "nothing";
            if (task.Items != null && task.Items.Any())
            {
                retriveStr = string.Join(", ", task.Items.Select(v => $"{v.Item.Amount} x {ItemDefinition.GetItemName(v.Item.ItemId)}"));
            }
            task.CompletionMessage = task.CompletionMessage.Replace("{1}", retriveStr);
            return task;
        }

        public SkillData GetForName(string name, string skill)
        {
            return SkillingData.FirstOrDefault(t => t.Skill.Equals(skill, StringComparison.InvariantCultureIgnoreCase) && (t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
            || t.OtherNames != null && t.OtherNames.Any(other => other.Equals(name, StringComparison.InvariantCultureIgnoreCase))));
        }

        public void Start()
        {
            SkillingData = ResourceExtensions.DeserializeResource<List<SkillData>>(typeof(SkillSimulator), "RS3Bot.Cli.Data.skillData.json");
        }
    }
}
