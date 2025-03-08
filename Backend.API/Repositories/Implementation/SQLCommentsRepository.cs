using Backend.API.Data;
using Backend.API.Models.Domain;
using Backend.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.Repositories.Implementation
{
    public class SQLCommentsRepository : ICommentsRepository
    {
        private readonly BackendDbContext dbContext;

        public SQLCommentsRepository(BackendDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(Guid id)
        {
            var comment = await dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
                return null;

            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await dbContext.Comments.Include("User").Include("Post").ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await dbContext.Comments.Include("User").Include("Post").FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Comment>> GetByPostIdAsync(Guid postId)
        {
            return await dbContext.Comments.Include("User").Include("Post").Where(c => c.PostId == postId).ToListAsync();
        }

        public async Task<Comment?> UpdateAsync(Comment comment, Guid id)
        {
            var existingComment = await dbContext.Comments.Include("User").Include("Post").FirstOrDefaultAsync(c => c.Id == id);

            if (existingComment == null)
                return null;

            dbContext.Entry(existingComment).CurrentValues.SetValues(comment);
            await dbContext.SaveChangesAsync();
            return existingComment;
        }
    }
}
