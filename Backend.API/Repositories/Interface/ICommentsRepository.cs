using Backend.API.Models.Domain;

namespace Backend.API.Repositories.Interface
{
    public interface ICommentsRepository
    {
        public Task<List<Comment>> GetAllAsync();
        public Task<Comment?> GetByIdAsync(Guid id);
        public Task<List<Comment>> GetByPostIdAsync(Guid postId);
        public Task<Comment> CreateAsync(Comment comment);
        public Task<Comment?> UpdateAsync(Comment comment, Guid id);
        public Task<Comment?> DeleteAsync(Guid id);
    }
}
