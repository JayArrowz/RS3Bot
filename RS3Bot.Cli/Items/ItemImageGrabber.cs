﻿using RS3BotWeb.Shared;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace RS3Bot.Cli
{
    public class ItemImageGrabber : IItemImageGrabber
    {
        private static readonly string ApiUrl = "https://secure.runescape.com/m=itemdb_rs/obj_sprite.gif?id={0}";
        private static readonly string CacheDir = "./cache/images/";
        private readonly IHttpClientFactory _httpClientFactory;
        private object _cacheLock = new object();

        public ItemImageGrabber(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            if (!Directory.Exists(CacheDir))
            {
                Directory.CreateDirectory(CacheDir);
            }
        }

        private static Bitmap ImageTrim(Bitmap img)
        {
            //get image data
            BitmapData bd = img.LockBits(new Rectangle(Point.Empty, img.Size),
            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int[] rgbValues = new int[img.Height * img.Width];
            Marshal.Copy(bd.Scan0, rgbValues, 0, rgbValues.Length);
            img.UnlockBits(bd);


            #region determine bounds
            int left = bd.Width;
            int top = bd.Height;
            int right = 0;
            int bottom = 0;

            //determine top
            for (int i = 0; i < rgbValues.Length; i++)
            {
                int color = rgbValues[i] & 0xffffff;
                if (color != 0)
                {
                    int r = i / bd.Width;
                    int c = i % bd.Width;

                    if (left > c)
                    {
                        left = c;
                    }
                    if (right < c)
                    {
                        right = c;
                    }
                    bottom = r;
                    top = r;
                    break;
                }
            }

            //determine bottom
            for (int i = rgbValues.Length - 1; i >= 0; i--)
            {
                int color = rgbValues[i] & 0xffffff;
                if (color != 0)
                {
                    int r = i / bd.Width;
                    int c = i % bd.Width;

                    if (left > c)
                    {
                        left = c;
                    }
                    if (right < c)
                    {
                        right = c;
                    }
                    bottom = r;
                    break;
                }
            }

            if (bottom > top)
            {
                for (int r = top + 1; r < bottom; r++)
                {
                    //determine left
                    for (int c = 0; c < left; c++)
                    {
                        int color = rgbValues[r * bd.Width + c] & 0xffffff;
                        if (color != 0)
                        {
                            if (left > c)
                            {
                                left = c;
                                break;
                            }
                        }
                    }

                    //determine right
                    for (int c = bd.Width - 1; c > right; c--)
                    {
                        int color = rgbValues[r * bd.Width + c] & 0xffffff;
                        if (color != 0)
                        {
                            if (right < c)
                            {
                                right = c;
                                break;
                            }
                        }
                    }
                }
            }

            int width = right - left + 1;
            int height = bottom - top + 1;
            #endregion

            //copy image data
            int[] imgData = new int[width * height];
            for (int r = top; r <= bottom; r++)
            {
                Array.Copy(rgbValues, r * bd.Width + left, imgData, (r - top) * width, width);
            }

            //create new image
            Bitmap newImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData nbd
                = newImage.LockBits(new Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(imgData, 0, nbd.Scan0, imgData.Length);
            newImage.UnlockBits(nbd);
            newImage.MakeTransparent(Color.Black);
            return newImage;
        }

        public async Task<Stream> GetAsync(int id)
        {
            var file = Path.Combine(CacheDir, id + ".png");
            if (File.Exists(file))
            {
                return File.OpenRead(file);
            }

            var client = _httpClientFactory.CreateClient();
            {
                var response = await client.GetAsync(string.Format(ApiUrl, id));
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    lock (_cacheLock)
                    {
                        if (!File.Exists(file))
                        {
                            using (var image = Image.FromStream(stream))
                            using (var imageBitmap = new Bitmap(image))
                            using (var croppedBitmap = ImageTrim(imageBitmap))
                            using (var fileStream = File.Create(file))
                            {
                                var memoryStream = new MemoryStream();
                                croppedBitmap.Save(fileStream, ImageFormat.Png);

                                fileStream.Position = 0;
                                fileStream.CopyTo(memoryStream);
                                memoryStream.Position = 0;
                                return memoryStream;
                            }
                        }
                        else
                        {
                            return File.OpenRead(file);
                        }
                    }
                }
            }
        }
    }
}
