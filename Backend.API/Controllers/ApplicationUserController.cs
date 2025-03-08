using AutoMapper;
using Backend.API.Models.Domain;
using Backend.API.Models.DTOs;
using Backend.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IApplicationUserRepository applicationUserRepository;

        public ApplicationUserController(IMapper mapper, IApplicationUserRepository applicationUserRepository)
        {
            this.mapper = mapper;
            this.applicationUserRepository = applicationUserRepository;
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var user = await applicationUserRepository.GetByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ApplicationUserDto>(user));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await applicationUserRepository.GetAllAsync();
            return Ok(mapper.Map<List<ApplicationUserDto>>(users));
        }
    }
}
