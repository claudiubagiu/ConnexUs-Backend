using Backend.API.Data;
using Backend.API.Models.Domain;
using Backend.API.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.Repositories.Implementation
{
    public class SQLPostsRepository : IPostsRepository
    {
        private readonly BackendDbContext dbContext;

        public SQLPostsRepository(BackendDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Post> CreateAsync(Post post)
        {
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
            return post;
        }

        public async Task<Post?> DeleteAsync(Guid id)
        {
            var post = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return null;
            }
            dbContext.Posts.Remove(post);
            await dbContext.SaveChangesAsync();
            return post;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await dbContext.Posts.Include("User").ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(Guid id)
        {
            return await dbContext.Posts.Include("User").FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post?> UpdateAsync(Post post)
        {
            var existingPost = await dbContext.Posts.Include("User").FirstOrDefaultAsync(p => p.Id == post.Id);

            if (existingPost == null)
            {
                return null;
            }

            dbContext.Entry(existingPost).CurrentValues.SetValues(post);

            await dbContext.SaveChangesAsync();
            return existingPost;
        }
    }
}
