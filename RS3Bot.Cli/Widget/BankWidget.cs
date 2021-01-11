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
using System.Threading.Tasks;
using static RS3Bot.Cli.Widget.BankWidget;

namespace RS3Bot.Cli.Widget
{
    public class BankWidget : IWidget<BankWidgetOptions>
    {
        public class BankWidgetOptions
        {
            public string Title { get; set; }
            public List<UserItem> Items { get; set; }
            public BankOption Options { get; set; }
        }

        private readonly PrivateFontCollection _fontCollection;
        private readonly IItemImageGrabber _itemImageGrabber;

        public BankWidget(PrivateFontCollection fontCollection, IItemImageGrabber itemImageGrabber)
        {
            _fontCollection = fontCollection;
            _itemImageGrabber = itemImageGrabber;
        }

        public async Task<Stream> GetWidgetAsync(BankWidgetOptions bankWidgetOptions)
        {
            var memoryStream = ResourceExtensions.GetStreamCopy(typeof(Program), "RS3Bot.Cli.Images.Bank_Interface.png");
            using (var backInterface = new Bitmap(Image.FromStream(memoryStream)))
            {
                var bankInterfaceHeight = backInterface.Height;

                int amountCount = 0;

                // Set format of string.
                StringFormat drawFormat = new StringFormat();
                drawFormat.FormatFlags = StringFormatFlags.NoWrap;
                var maxItemsPerRow = 12;
                var maxItemSquareSize = 32;
                using (var font = new Font(_fontCollection.Families[0], 8))
                using (var titleFont = new Font(_fontCollection.Families[0], 12))
                {
                    using (Graphics g = Graphics.FromImage(backInterface))
                    {
                        var bankTitle = bankWidgetOptions.Title;
                        var size = g.MeasureString(bankTitle, titleFont);
                        g.DrawString(bankTitle, titleFont, Brushes.Gold, 280 - (size.Width / 2), size.Height + 2);
                        foreach (var item in bankWidgetOptions.Items)
                        {
                            using (var rawStream = await _itemImageGrabber.GetAsync(item.Item.ItemId))
                            {
                                using (var imageStream = Image.FromStream(rawStream))
                                {
                                    using (SolidBrush drawBrush = new SolidBrush(StackFormatter.GetColor(item.Item.Amount)))
                                    {
                                        var row = (int)Math.Floor(amountCount / (double)maxItemsPerRow);
                                        var itemX = 18 + ((amountCount % maxItemsPerRow) * 45);
                                        int itemY = 55 + (row * 40);

                                        var horizontalCenter = itemY + ((maxItemSquareSize - imageStream.Height) / 2);
                                        var verticalCenter = itemX + ((maxItemSquareSize - imageStream.Width) / 2);

                                        g.DrawImage(imageStream, verticalCenter, horizontalCenter, imageStream.Width, imageStream.Height);
                                        g.DrawString(StackFormatter.QuantityToRSStackSize((long)item.Item.Amount), font, drawBrush,
                                            itemX - 5,
                                            itemY - 7,
                                            drawFormat);

                                        if (bankWidgetOptions.Options.ShowId)
                                        {
                                            g.DrawString(item.Item.ItemId.ToString(), font, Brushes.White,
                                               itemX - 10,
                                               itemY + 30,
                                               drawFormat);
                                        }
                                        amountCount += 1;
                                    }

                                }
                            }
                        }
                    }
                }
                memoryStream.Position = 0;
                backInterface.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Position = 0;
                return memoryStream;
            }
        }
    }
}
