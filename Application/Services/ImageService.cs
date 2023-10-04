using System;
using System.IO;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Services
{
    public class ImageService : IImageService
    {
        private readonly string _imageDirectory = "wwwroot/images";

        public ImageService()
        {
        }

        public async Task<string> SaveImageAsync(string imageData, string recipeName)
        {
            if (string.IsNullOrEmpty(imageData))
                throw new ArgumentException("Image data cannot be null or empty.");
            
            // image data is somehting like: "data:image/jpeg;base64,/9j/4AAQSkZJR..."
            // we need to extract image format & base64 data from the input string
            var base64Data = imageData.Split(',')[1];
            var imageFormat = imageData.Split(',')[0].Split('/')[1].Split(';')[0];

            var imageName = $"{recipeName}-{Guid.NewGuid()}.{imageFormat}";

            var imagePath = Path.Combine(_imageDirectory, imageName);

            // convert base64 data to byte array
            var bytes = Convert.FromBase64String(base64Data);

            // save the image 
            await File.WriteAllBytesAsync(imagePath, bytes);

            return imageName;
        }
    }
}