using Backend.API.Data;
using Backend.API.Models.Domain;
using Backend.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.Repositories.Implementation
{
    public class SQLApplicationUserRepository : IApplicationUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public SQLApplicationUserRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await userManager.FindByIdAsync(id);
        }
    }
}
