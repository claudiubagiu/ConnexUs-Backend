using AutoMapper;
using Backend.API.CustomActionFilters;
using Backend.API.Models.Domain;
using Backend.API.Models.DTOs;
using Backend.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly IMapper mapper;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.mapper = mapper;
        }

        // POST : /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        [ValidateModel]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var user = mapper.Map<ApplicationUser>(registerRequestDto);


            var result = await userManager.CreateAsync(user, registerRequestDto.Password);

            if (result.Succeeded)
            {
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    result = await userManager.AddToRolesAsync(user, registerRequestDto.Roles);

                    if(result.Succeeded)
                    {
                        return Ok(new { message = "User was registered! Please login." });
                    }
                }
            }
            return BadRequest(new { message = "Something went wrong! Please try again." });
        }

        // POST : /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Email);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken,
                            Email = loginRequestDto.Email,
                            Roles = roles.ToList()
                        };
                        return Ok(response);
                    }
                }
            }
            return BadRequest("Username or password incorrect!");
        }
    }
}
