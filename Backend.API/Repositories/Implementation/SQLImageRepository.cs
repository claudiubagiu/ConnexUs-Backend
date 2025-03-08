using Backend.API.Data;
using Backend.API.Models.Domain;
using Backend.API.Repositories.Interface;

namespace Backend.API.Repositories.Implementation
{
    public class SQLImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly BackendDbContext dbContext;

        public SQLImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor,
            BackendDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // https ://localhost:1234/images/image.jpg
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            // Add image to the Images table
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;

        }
    }
}
