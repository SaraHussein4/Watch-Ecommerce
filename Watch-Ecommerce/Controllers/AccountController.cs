using AutoMapper;
using ECommerce.Core.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Watch_Ecommerce.DTOs;
using Watch_Ecommerce.Services;

namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<User> userManager , SignInManager<User> signInManager , ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("Register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
            {
                return BadRequest("This Email Is Already Exist");
            }

            var user = new User()
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };
            var Result = await _userManager.CreateAsync(user, model.Password);
            //await _userManager.AddToRoleAsync(user, "User");
            if (!Result.Succeeded)
            {
                var errors = Result.Errors.Select(e => e.Description).ToArray();
                return BadRequest(new
                {
                    statusCode = 400,
                    errors = errors
                });
            }
            await _userManager.AddToRoleAsync(user, "User");

            var returnerUser = new UserDto()
            {
                Name = model.Name,
                Email = model.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
                Role = "User"

            };
            return Ok(returnerUser);
        
        }

        [HttpPost("Login")]

        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";
            if (user is null) return Unauthorized();
            var Result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!Result.Succeeded) return Unauthorized();
            return new UserDto
            {
                Name = user.Name,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
                Role = role
            };
        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {

            return await _userManager.FindByEmailAsync(email) is not null;

        }


        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "User logged out successfully." });
        }

    }
}
