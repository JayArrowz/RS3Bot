using Discord.WebSocket;
using RS3Bot.Abstractions.Constants;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using RS3Bot.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RS3Bot.Cli.Widget.EquipmentWidget;

namespace RS3Bot.Cli.Commands
{
    public class GearCommand : UserAwareCommand<GearOption>
    {
        private readonly IWidget<EquipmentWidgetOptions> _equipmentWidget;

        public GearCommand(IWidget<EquipmentWidgetOptions> equipmentWidget, IContextFactory contextFactory) : base(contextFactory)
        {
            _equipmentWidget = equipmentWidget;
        }


        protected override async Task<bool> ExecuteCommand(IDiscordBot bot, SocketMessage message, ApplicationUser user, ApplicationDbContext context, GearOption option)
        {
            List<EquipmentItem> equipmentItems = user.Equipment.Where(t => t.EquipmentType.Equals(option.EquipmentType, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();

            if (!EquipmentConstants.IsValidEquipmentType(option.EquipmentType))
            {
                await message.Channel.SendMessageAsync($"Invalid command equipment type {option.EquipmentType} from {message.Author}");
                return false;
            }

            using (var equipmentWidget =
               await _equipmentWidget.GetWidgetAsync(
                   new EquipmentWidgetOptions { Options = option, Items = equipmentItems }))
            {
                await message.Channel.SendFileAsync(equipmentWidget, "image.png", string.Empty);
            }
            return true;
        }
    }
}
