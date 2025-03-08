namespace Backend.API.Models.DTOs
{
    public class UpdatePostRequestDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string FeaturedImageUrl { get; set; }
    }
}
