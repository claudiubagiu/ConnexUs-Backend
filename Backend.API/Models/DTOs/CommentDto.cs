using Backend.API.Models.Domain;

namespace Backend.API.Models.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }

        public string UserId { get; set; }
        public ApplicationUserDto User { get; set; }

        public Guid PostId { get; set; }
        public PostDto Post { get; set; }
    }
}
