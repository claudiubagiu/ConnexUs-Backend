using Backend.API.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Models.DTOs
{
    public class AddPostRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Title has to be a minimum of 3 characters.")]
        [MaxLength(50, ErrorMessage = "Title has to be a maximum of 50 characters.")]
        public string Title { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Content has to be a minimum of 3 characters.")]
        [MaxLength(1000, ErrorMessage = "Content has to be a maximum of 50 characters.")]
        public string Content { get; set; }
        [Required]
        public string FeaturedImageUrl { get; set; }
    }
}
