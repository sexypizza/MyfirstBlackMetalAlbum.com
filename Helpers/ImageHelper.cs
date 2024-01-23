using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;


namespace MyfirstBlackMetalAlbum.com.Helpers
{
    public static class ImageHelper
    {
        public static byte[] ResizeImage(byte[] imageData, int width, int height, string format)
        {
            using (var image = Image.Load<Rgba32>(imageData))
            {
                image.Mutate(x => x.Resize(width, height));
                using (var stream = new MemoryStream())
                {
                    if (format.ToLower() == "png")
                    {
                        image.SaveAsPng(stream);
                    }
                    else if (format.ToLower() == "jpg" || format.ToLower() == "jpeg")
                    {
                        image.SaveAsJpeg(stream);
                    }
                    else
                    {
                        // Default to JPEG if the format is unknown
                        image.SaveAsJpeg(stream);
                    }
                    return stream.ToArray();
                }
            }
        }
    }
}