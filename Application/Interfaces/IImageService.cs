using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(string imageData, string recipeName);
    }
}
