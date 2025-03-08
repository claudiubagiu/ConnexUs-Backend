using AutoMapper;
using Backend.API.Models.Domain;
using Backend.API.Models.DTOs;
using Backend.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;
        private readonly IMapper mapper;

        public ImagesController(IImageRepository imageRepository, IMapper mapper)
        {
            this.imageRepository = imageRepository;
            this.mapper = mapper;
        }

        // POST : /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);
            if (ModelState.IsValid)
            {
                var imageDomainModel = mapper.Map<Image>(request);

                await imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);

            }
            return Ok(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            var maxFileSizeInBytes = 10485760; // 10MB
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("File", "File extension is not allowed.");
            }
            if (request.File.Length > maxFileSizeInBytes)
            {
                ModelState.AddModelError("File", "File size is too large.");
            }
        }
    }
}
