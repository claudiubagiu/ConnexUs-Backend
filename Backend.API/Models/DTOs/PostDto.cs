using Backend.API.Models.Domain;

namespace Backend.API.Models.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string FeaturedImageUrl { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUserDto User { get; set; }
    }
}
