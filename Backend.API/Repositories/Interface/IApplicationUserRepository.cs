using Backend.API.Models.Domain;

namespace Backend.API.Repositories.Interface
{
    public interface IApplicationUserRepository
    {
        Task<List<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(string id);    }
}
