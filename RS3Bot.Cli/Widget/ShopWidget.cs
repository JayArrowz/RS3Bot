using RS3Bot.Abstractions.Extensions;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli.Commands.Options;
using RS3BotWeb.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static RS3Bot.Cli.Widget.ShopWidget;

namespace RS3Bot.Cli.Widget
{
    public class ShopWidget : IWidget<ShopWidgetOptions>
    {
        private static readonly int MaxRowSize = 3;
        private static readonly int YSpacing = 65;
        private static readonly int FooterHeight = 17;
        private static readonly int XSpacing = 153;
        private readonly IItemImageGrabber _imageGrabber;
        private readonly PrivateFontCollection _fontCollection;
        private static readonly int MaxItemChar = 12;

        public class ShopWidgetOptions
        {
            public List<ShopItem> Items { get; set; }
            public string Title { get; set; }
        }

        public ShopWidget(IItemImageGrabber imageGrabber, PrivateFontCollection fontCollection)
        {
            _imageGrabber = imageGrabber;
            _fontCollection = fontCollection;
        }
        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }


        public async Task<Stream> GetWidgetAsync(ShopWidgetOptions lootWidgetOptions)
        {
            var headerStream = ResourceExtensions.GetStreamCopy(typeof(Program), "RS3Bot.Cli.Images.Shop_Header.png");
            using (var headerImage = Image.FromStream(headerStream))
            using (var footerStream = ResourceExtensions.GetStreamCopy(typeof(Program), "RS3Bot.Cli.Images.Shop_Footer.png"))
            using (var footerImage = Image.FromStream(footerStream))
            {
                var rowAmount = (int)Math.Ceiling(lootWidgetOptions.Items.Count / (double)MaxRowSize);
                var headerHeight = headerImage.Height;
                using (var lootImage = new Bitmap(headerImage.Width, headerHeight + (rowAmount * YSpacing) + FooterHeight))
                using (var font = new Font(_fontCollection.Families[0], 8))
                using (var shopItem = ResourceExtensions.GetStreamCopy(typeof(Program), "RS3Bot.Cli.Images.Shop_Item.png"))
                using (var rowStream = ResourceExtensions.GetStreamCopy(typeof(Program), "RS3Bot.Cli.Images.Shop_Row.png"))
                using (var rowImage = Image.FromStream(rowStream))
                using (var shopItemImage = Image.FromStream(shopItem))
                using (var titleFont = new Font(_fontCollection.Families[0], 40))
                using (Graphics g = Graphics.FromImage(lootImage))
                {
                    g.DrawImage(headerImage, 0, 0);
                    g.DrawString(lootWidgetOptions.Title, font, Brushes.Gold, 26, 12);
                    for (int i = 0; i < lootWidgetOptions.Items.Count; i++)
                    {
                        var row = (int)Math.Floor(i / (double)MaxRowSize);
                        var addNewRow = i % MaxRowSize == 0;
                        var itemX = 30 + ((i % MaxRowSize) * XSpacing);
                        var itemY = 80 + (row * YSpacing);
                        var item = lootWidgetOptions.Items[i];
                        if (addNewRow)
                        {
                            g.DrawImage(rowImage, 0, headerHeight + (row * YSpacing));
                        }

                        using (var itemStream = await _imageGrabber.GetAsync(item.Item.ItemId))
                        using (var imageStream = Image.FromStream(itemStream))
                        using (SolidBrush drawBrush = new SolidBrush(StackFormatter.GetColor(item.Item.Amount)))
                        {
                            var itemName = ItemDefinition.GetItemName(item.Item.ItemId);
                            var itemNameLines = Split(itemName, MaxItemChar);
                            var horizontalCenter = itemY + ((32 - imageStream.Height) / 2);
                            var verticalCenter = itemX + ((32 - imageStream.Width) / 2);

                            g.DrawImage(shopItemImage, itemX - 20, itemY - 15);
                            g.DrawImage(imageStream, verticalCenter, horizontalCenter, imageStream.Width, imageStream.Height);
                            g.DrawString(StackFormatter.QuantityToRSStackSize((long)item.Item.Amount), font, drawBrush,
                                            itemX - 7,
                                            itemY - 4);

                            g.DrawString(itemName, font, Brushes.White,
                                           new RectangleF(new PointF(itemX + 34f, itemY - 0f), new SizeF(95, 36)));


                            var amountStr = StackFormatter.QuantityToRSStackSize(item.Price);
                            var amountMeasure = g.MeasureString(amountStr, font);
                            g.DrawString(amountStr, font, Brushes.White,
                                           itemX + 113 - (amountMeasure.Width / 2),
                                           itemY + 26);

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
