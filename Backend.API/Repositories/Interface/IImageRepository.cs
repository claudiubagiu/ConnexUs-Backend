using Backend.API.Models.Domain;

namespace Backend.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
