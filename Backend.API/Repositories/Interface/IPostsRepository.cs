using Backend.API.Models.Domain;

namespace Backend.API.Repositories.Interface
{
    public interface IPostsRepository
    {
        Task<Post> CreateAsync(Post post);
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(Guid id);
        Task<Post?> DeleteAsync(Guid id);
        Task<Post?> UpdateAsync(Post post);
    }
}
