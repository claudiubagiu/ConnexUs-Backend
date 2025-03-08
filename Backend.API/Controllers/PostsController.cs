using AutoMapper;
using Backend.API.CustomActionFilters;
using Backend.API.Models.Domain;
using Backend.API.Models.DTOs;
using Backend.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPostsRepository postsRepository;
        private readonly IApplicationUserRepository applicationUserRepository;

        public PostsController(IMapper mapper, IPostsRepository postsRepository, IApplicationUserRepository applicationUserRepository)
        {
            this.mapper = mapper;
            this.postsRepository = postsRepository;
            this.applicationUserRepository = applicationUserRepository;
        }

        // POST : /api/Posts
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Create([FromBody] AddPostRequestDto addPostRequestDto)
        {
            var userIdClaim = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing from token.");
            }

            var userId = userIdClaim.Value;

            var post = mapper.Map<Post>(addPostRequestDto);

            post.UserId = userId;

            var user = await applicationUserRepository.GetByIdAsync(userId);

            post.User = user;

            post = await postsRepository.CreateAsync(post);

            var postDto = mapper.Map<PostDto>(post);

            return CreatedAtAction(nameof(GetById), new { id = post.Id }, postDto);
        }

        // GET : /api/Posts/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var post = await postsRepository.GetByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<PostDto>(post));
        }

        // GET : /api/Posts
        [HttpGet]
        [ValidateModel]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAll()
        {
            var posts = await postsRepository.GetAllAsync();
            return Ok(mapper.Map<List<PostDto>>(posts));
        }

        // PUT : /api/Posts/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePostRequestDto updatePostRequestDto)
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaims = HttpContext.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (userIdClaim == null)
                return Unauthorized("User ID is missing from token.");

            var userId = userIdClaim.Value;

            var post = await postsRepository.GetByIdAsync(id);

            if (post == null)
                return NotFound();

            if (userRoleClaims.Contains("Admin"))
                post = mapper.Map(updatePostRequestDto, post);
            else if (userRoleClaims.Contains("User"))
            {
                if (post.UserId != userId)
                    return Unauthorized("User is not authorized to update this post.");

                post = mapper.Map(updatePostRequestDto, post);
            }
            else
                return Forbid("Invalid role.");

            post = await postsRepository.UpdateAsync(post);

            return Ok(mapper.Map<PostDto>(post));
        }

        // DELETE : /api/Posts/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaims = HttpContext.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (userIdClaim == null)
                return Unauthorized("User ID is missing from token.");

            var userId = userIdClaim.Value;

            var post = await postsRepository.GetByIdAsync(id);

            if (post == null)
                return NotFound();

            if (userRoleClaims.Contains("Admin"))
                post = await postsRepository.DeleteAsync(id);
            else if (userRoleClaims.Contains("User"))
            {
                if (post.UserId != userId)
                    return Unauthorized("User is not authorized to delete this post.");
                post = await postsRepository.DeleteAsync(id);
            }
            else
                return Forbid("Invalid role.");

            return Ok(mapper.Map<PostDto>(post));
        }
    }
}
