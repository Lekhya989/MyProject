using ApptManager.Models;
using ApptManager.Repo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApptManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            this._userService = userService;

        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await this._userService.GetAll();
            if (users != null)
            {
                return Ok(users);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("GetbyCode/{Id}")]
        public async Task<IActionResult> GetbyCode(int Id)
        {
            var users = await this._userService.GetbyId(Id);
            if (users != null)
            {
                return Ok(users);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UserObj user)
        {
            var _result = await this._userService.Create(user);
            return Ok(_result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(UserObj user, int Id)
        {
            var _result = await this._userService.Update(user, Id);
            return Ok(_result);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int Id)
        {
            var _result = await this._userService.Remove(Id);
            return Ok(_result);
        }

        [HttpGet("test-email")]
        public async Task<IActionResult> TestEmail([FromServices] IMailService mailService)
        {
            await mailService.SendEmailAsync(new MailRequest
            {
                ToEmail = "lekhyachowdary1@gmail.com",
                Subject = "Test Email",
                Body = "If you're reading this, it worked!"
            });

            return Ok("Email sent");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, [FromServices] JwtTokenService tokenService)
        {
            var user = await _userService.GetByEmail(loginRequest.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
                return Unauthorized("Invalid email or password");

            var token = tokenService.GenerateToken(user.Id,user.Email, user.UserType.ToString());

            return Ok(new LoginResponse
            {
                Email = user.Email,
                UserType = user.UserType.ToString(),
                Token = token
            });
        }


    }

}
