using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.API.Models.Domain
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string FeaturedImageUrl { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
