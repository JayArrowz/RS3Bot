using RS3Bot.Abstractions.Extensions;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Options;
using RS3BotWeb.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RS3Bot.Cli.Widget.LootWidget;

namespace RS3Bot.Cli.Widget
{
    public class LootWidget : IWidget<LootWidgetOptions>
    {
        private static readonly int MaxRowSize = 5;
        private static readonly int RowHeight = 38;
        private static readonly int FooterHeight = 59;
        private readonly IItemImageGrabber _imageGrabber;
        private readonly PrivateFontCollection _fontCollection;

        public class LootWidgetOptions
        {
            public List<UserItem> Items { get; set; }
            public BankOption Options { get; set; }
        }

        public LootWidget(IItemImageGrabber imageGrabber, PrivateFontCollection fontCollection)
        {
            _imageGrabber = imageGrabber;
            _fontCollection = fontCollection;
        }

        public async Task<Stream> GetWidgetAsync(LootWidgetOptions lootWidgetOptions)
        {
            var headerStream = ResourceExtensions.GetStreamCopy(typeof(CliParser), "RS3Bot.Cli.Images.Loot_Head.png");
            using (var headerImage = Image.FromStream(headerStream))
            using (var footerStream = ResourceExtensions.GetStreamCopy(typeof(CliParser), "RS3Bot.Cli.Images.Loot_Footer.png"))
            using (var footerImage = Image.FromStream(footerStream))
            {
                var rowAmount = (int)Math.Ceiling(lootWidgetOptions.Items.Count / (double)MaxRowSize);
                var headerHeight = headerImage.Height;
                using (var lootImage = new Bitmap(headerImage.Width, headerHeight + (rowAmount * RowHeight) + FooterHeight))
                using (var font = new Font(_fontCollection.Families[0], 8))
                using (var titleFont = new Font(_fontCollection.Families[0], 12))
                using (Graphics g = Graphics.FromImage(lootImage))
                {
                    g.DrawImage(headerImage, 0, 0);
                    for (int i = 0; i < lootWidgetOptions.Items.Count; i++)
                    {
                        var row = (int)Math.Floor(i / (double)MaxRowSize);
                        var addNewRow = i % MaxRowSize == 0;
                        var itemX = 20 + ((i % MaxRowSize) * 45);
                        var itemY = 45 + (row * RowHeight);
                        var item = lootWidgetOptions.Items[i];
                        if (addNewRow)
                        {
                            using (var rowStream = ResourceExtensions.GetStreamCopy(typeof(CliParser), "RS3Bot.Cli.Images.Loot_Row.png"))
                            using (var rowImage = Image.FromStream(rowStream))
                            {
                                g.DrawImage(rowImage, 0, headerHeight + (row * RowHeight));
                            }
                        }

                        using (var itemStream = await _imageGrabber.GetAsync(item.ItemId))
                        using (var imageStream = Image.FromStream(itemStream))
                        using (SolidBrush drawBrush = new SolidBrush(StackFormatter.GetColor((long)item.Amount)))
                        {
                            var horizontalCenter = itemY + ((32 - imageStream.Height) / 2);
                            var verticalCenter = itemX + ((32 - imageStream.Width) / 2);
                            g.DrawImage(imageStream, verticalCenter, horizontalCenter, imageStream.Width, imageStream.Height);
                            g.DrawString(StackFormatter.QuantityToRSStackSize((long)item.Amount), font, drawBrush,
                                            itemX - 7,
                                            itemY - 4);

                        }
                    }

                    g.DrawImage(footerImage, 0, lootImage.Height - FooterHeight);
                    headerStream.Position = 0;
                    lootImage.Save(headerStream, System.Drawing.Imaging.ImageFormat.Png);
                    headerStream.Position = 0;
                }
                return headerStream;
            }
        }
    }
}
