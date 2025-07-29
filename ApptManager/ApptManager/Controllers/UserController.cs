using ApptManager.DTOs;
using ApptManager.Models;
using ApptManager.Repo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ApptManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMailService mailService, IMapper mapper)
        {
            _userService = userService;
            _mailService = mailService;
            _mapper = mapper;
        }




        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            var msg = await _userService.Create(dto);
            Log.Information("User Created");
            return Ok(new { message = msg });
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(
          [FromBody] LoginRequestDto loginRequest,
          [FromServices] JwtTokenService tokenService)
        {
            var user = await _userService.GetByEmail(loginRequest.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                Log.Information("Invalid login attempt.");
                return Unauthorized("Invalid email or password");
            }

            var accessToken = tokenService.GenerateToken(user.Id, user.Email, user.UserType.ToString());
            var refreshToken = Guid.NewGuid().ToString(); 

            Log.Information("User logged in and tokens issued.");

            return Ok(new
            {
                accessToken,
                refreshToken,
                email = user.Email,
                userId = user.Id,
                userType = user.UserType.ToString()
            });
        }

        // Protected: get all users

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            var dtos = _mapper.Map<IEnumerable<UserResponseDto>>(users);

            Log.Information("Fetched all users");
            return dtos.Any() ? Ok(dtos) : NotFound("No users found");
        }


        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
                return NotFound("User not found");

            var dto = _mapper.Map<UserResponseDto>(user);
            Log.Information("Fetched user by ID");
            return Ok(dto);
        }



        // Protected: update an existing user

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto dto, int id)
        {
            var user = await _userService.GetById(id);
            if (user == null) return NotFound("User not found");

            _mapper.Map(dto, user); // Apply changes
            var msg = await _userService.Update(dto, id);

            Log.Information("User updated");
            return Ok(new { message = msg });
        }


        // Protected: delete a user

        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var msg = await _userService.Remove(id);
            Log.Information("user removed");
            return Ok(new { message = msg });
        }

       
      
        [HttpGet("TestMail")]
        public async Task<IActionResult> TestMail()
        {
            var request = new MailRequestDto
            {
                ToEmail = "yourtestmail@gmail.com",
                Subject = "Test Email",
                Body = "<h1>This is a test email from the server</h1>"
            };

            try
            {
                await _mailService.SendEmailAsync(request);
                Log.Information("Email sent");
                return Ok("Email sent");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken(
             [FromBody] RefreshTokenRequestDto request,
             [FromServices] JwtTokenService tokenService)
        {
            if (string.IsNullOrEmpty(request.RefreshToken) ||
                string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.UserType) ||
                string.IsNullOrEmpty(request.UserId))
            {
                Log.Warning("Missing refresh token or user data.");
                return Unauthorized("Invalid request.");
            }

            if (!int.TryParse(request.UserId, out int userId))
            {
                Log.Warning("Invalid user ID.");
                return Unauthorized("Invalid user ID.");
            }

            var newAccessToken = tokenService.GenerateToken(userId, request.Email, request.UserType);

            Log.Information("Access token refreshed.");
            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = request.RefreshToken 
            });
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Log.Information("User logged out.");
            return Ok(new { message = "Logged out successfully." });
        }

    }
}
