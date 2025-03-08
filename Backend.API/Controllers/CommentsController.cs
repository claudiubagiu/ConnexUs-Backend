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
    public class CommentsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICommentsRepository commentsRepository;
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly IPostsRepository postsRepository;

        public CommentsController(IMapper mapper, ICommentsRepository commentsRepository, IApplicationUserRepository applicationUserRepository, IPostsRepository postsRepository)
        {
            this.mapper = mapper;
            this.commentsRepository = commentsRepository;
            this.applicationUserRepository = applicationUserRepository;
            this.postsRepository = postsRepository;
        }

        // GET : /api/Comments
        [HttpGet]
        [ValidateModel]
        public async Task<IActionResult> GetAll()
        {
            var comments = await commentsRepository.GetAllAsync();

            var commentsDto = mapper.Map<List<CommentDto>>(comments);

            return Ok(commentsDto);
        }

        // GET : /api/Comments/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> GetById(Guid id)
        {
            var comment = await commentsRepository.GetByIdAsync(id);

            if (comment == null)
                return NotFound();

            var commentDto = mapper.Map<CommentDto>(comment);

            return Ok(commentDto);
        }

        // GET : /api/Comments/Post/{postId}
        [HttpGet]
        [Route("Post/{postId:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> GetByPostId(Guid postId)
        {
            var comments = await commentsRepository.GetByPostIdAsync(postId);

            if (comments == null)
                return NotFound();

            var commentsDto = mapper.Map<List<CommentDto>>(comments);

            return Ok(commentsDto);
        }

        // POST : /api/Comments/{postId}
        [HttpPost]
        [Route("{postId:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Create([FromBody] AddCommentRequestDto addCommentRequestDto, [FromRoute] Guid postId)
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing from token.");
            }

            var userId = userIdClaim.Value;

            var comment = mapper.Map<Comment>(addCommentRequestDto);

            comment.UserId = userId;
            comment.PostId = postId;

            var user = await applicationUserRepository.GetByIdAsync(userId);

            comment.User = user;

            var post = await postsRepository.GetByIdAsync(comment.PostId);

            comment.Post = post;

            comment = await commentsRepository.CreateAsync(comment);

            var commentDto = mapper.Map<CommentDto>(comment);

            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, commentDto);
        }

        // PUT : /api/Comments/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommentRequestDto updateCommentRequestDto)
        {
            var comment = await commentsRepository.GetByIdAsync(id);

            if (comment == null)
                return NotFound();

            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing from token.");
            }

            var userId = userIdClaim.Value;

            if (comment.UserId != userId)
                return Unauthorized("You are not authorized to update this comment.");

            comment = mapper.Map(updateCommentRequestDto, comment);

            comment = await commentsRepository.UpdateAsync(comment, id);

            if (comment == null)
                return NotFound();

            var commentDto = mapper.Map<CommentDto>(comment);

            return Ok(commentDto);
        }

        // DELETE : /api/Comments/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comment = await commentsRepository.GetByIdAsync(id);

            if (comment == null)
                return NotFound();

            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing from token.");
            }

            var userId = userIdClaim.Value;

            if (HttpContext.User.IsInRole("Admin"))
            {
                comment = await commentsRepository.DeleteAsync(id);

                if (comment == null)
                    return NotFound();

                var commentDto = mapper.Map<CommentDto>(comment);
                return Ok(commentDto);
            }

            if (comment.UserId != userId)
                return Unauthorized("You are not authorized to delete this comment.");

            comment = await commentsRepository.DeleteAsync(id);

            if (comment == null)
                return NotFound();

            var commentDtoUser = mapper.Map<CommentDto>(comment);
            return Ok(commentDtoUser);
        }
    }
}
