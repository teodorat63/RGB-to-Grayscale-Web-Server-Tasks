using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Projekat
{
    internal class ImageService
    {
        private MemoryCache cache;

        public ImageService()
        {
            cache = MemoryCache.Default;
        }

        public async Task<byte[]> ServeImageAsync(string filename)
        {
            try
            {
                byte[] cachedGrayscaleImage = cache.Get(filename) as byte[];
                if (cachedGrayscaleImage != null)
                {
                    return cachedGrayscaleImage;
                }

                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                {
                    byte[] imageData = new byte[fs.Length];
                    await fs.ReadAsync(imageData, 0, imageData.Length);

                    using (MemoryStream ms = new MemoryStream(imageData))
                    using (Image image = Image.FromStream(ms))
                    {
                        Bitmap grayscaleImage = await Task.Run(() => ConvertToGrayscale(image));

                        using (MemoryStream outputMs = new MemoryStream())
                        {
                            grayscaleImage.Save(outputMs, ImageFormat.Jpeg);
                            byte[] outputData = outputMs.ToArray();

                            cache.Set(filename, outputData, DateTimeOffset.Now.AddMinutes(10));

                            return outputData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serving image: {ex.Message}");
                throw new InvalidOperationException("Failed to serve image.", ex);
            }
        }

        private Bitmap ConvertToGrayscale(Image image)
        {
            Bitmap grayscaleImage = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = ((Bitmap)image).GetPixel(x, y);
                    int grayscaleValue = (int)(pixel.R * 0.299 + pixel.G * 0.587 + pixel.B * 0.114);
                    Color grayscaleColor = Color.FromArgb(grayscaleValue, grayscaleValue, grayscaleValue);
                    grayscaleImage.SetPixel(x, y, grayscaleColor);
                }
            }

            return grayscaleImage;
        }
    }
}
