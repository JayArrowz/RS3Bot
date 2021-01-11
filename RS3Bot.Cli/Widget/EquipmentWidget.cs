using RS3Bot.Abstractions.Constants;
using RS3Bot.Abstractions.Extensions;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using RS3BotWeb.Shared;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static RS3Bot.Cli.Widget.EquipmentWidget;

namespace RS3Bot.Cli.Widget
{
    public class EquipmentWidget : IWidget<EquipmentWidgetOptions>
    {
        private readonly IItemImageGrabber _imageGrabber;

        public EquipmentWidget(IItemImageGrabber imageGrabber)
        {
            _imageGrabber = imageGrabber;
        }

        public async Task<Stream> GetWidgetAsync(EquipmentWidgetOptions args)
        {
            var gridSpacing = 39;
            var helmetX = 90;
            var helmetY = 81;

            var amuletX = 90;
            var amuletY = helmetY + gridSpacing;

            var bodyX = 90;
            var bodyY = amuletY + gridSpacing;


            var weaponX = 29;
            var weaponY = bodyY;

            var helmetId = args.Items.FirstOrDefault(t => t.EquipmentSlot == EquipmentConstants.Hat);
            var amuletId = args.Items.FirstOrDefault(t => t.EquipmentSlot == EquipmentConstants.Amulet);
            var weaponId = args.Items.FirstOrDefault(t => t.EquipmentSlot == EquipmentConstants.Weapon);

            var equipmentBgStream = ResourceExtensions.GetStreamCopy(typeof(Program), "RS3Bot.Cli.Images.Equipment.png");
            using (var equipmentImage = Image.FromStream(equipmentBgStream))
            using (var bitmap = new Bitmap(equipmentImage))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                if (helmetId != null)
                {
                    await DrawItem(g, helmetId.Item.ItemId, helmetX, helmetY);
                }
                
                if (amuletId != null)
                {
                    await DrawItem(g, amuletId.Item.ItemId, amuletX, amuletY);
                }

                if(weaponId != null)
                {
                    await DrawItem(g, weaponId.Item.ItemId, weaponX, weaponY);
                }

                equipmentBgStream.Position = 0;
                bitmap.Save(equipmentBgStream, ImageFormat.Png);
                equipmentBgStream.Position = 0;
                return equipmentBgStream;
            }
        }

        private async Task DrawItem(Graphics g, int id, int x, int y)
        {
            using (var imageStream = await _imageGrabber.GetAsync(id))
            {
                using (var image = Image.FromStream(imageStream))
                {
                    var horizontalCenter = y + ((32 - image.Height) / 2);
                    var verticalCenter = x + ((32 - image.Width) / 2);
                    g.DrawImage(image, verticalCenter, horizontalCenter);
                }
            }
        }

        public class EquipmentWidgetOptions
        {
            public GearOption Options { get; set; }
            public List<EquipmentItem> Items { get; set; }
        }
    }
}
