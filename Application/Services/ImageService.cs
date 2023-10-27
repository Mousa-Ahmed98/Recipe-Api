using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Services
{
    public class ImageService : IImageService
    {
        private readonly string _imageDirectory = "wwwroot/images";

        public ImageService() { }

        public async Task<string> SaveImageAsync(string imageData, string recipeName)
        {
            if (string.IsNullOrEmpty(imageData))
                throw new ArgumentNullException("Image data cannot be null or empty.");
            
            // image data is somehting like: "data:image/jpeg;base64,/9j/4AAQSkZJR..."
            // we need to extract image format & base64 data from the input string
            var base64Data = imageData.Split(',')[1];
            var imageFormat = imageData.Split(',')[0].Split('/')[1].Split(';')[0];

            var imageName = $"{ recipeName.Replace(' ', '-') }-{GetRandomString()}.{imageFormat}";

            var imagePath = Path.Combine(_imageDirectory, imageName);

            // convert base64 data to byte array
            var bytes = Convert.FromBase64String(base64Data);

            // save the image 
            await File.WriteAllBytesAsync(imagePath, bytes);

            return imageName;
        }

        public void RemoveImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                throw new ArgumentNullException("Image Name cannot be null or empty.");
            
            var imageFile = Path.Combine(_imageDirectory, imageName);

            if (File.Exists(imageFile))
            {
                File.Delete(imageFile);
            }
        }

        private static string GetRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var str = new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return str;
        }

    }
}