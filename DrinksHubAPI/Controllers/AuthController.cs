using DrinksHubAPI.DataAccess;
using DrinksHubAPI.DTOs;
using DrinksHubAPI.Helpers;
using DrinksHubAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace DrinksHubAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserRepository _userRepository;
		private readonly IConfiguration _config;

		public AuthController(IUserRepository userRepository, IConfiguration config)
		{
			_userRepository = userRepository;
			_config = config;
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginRequestDTO dto)
		{
			var user = await _userRepository.GetByUsernameAsync(dto.Username);

			if (user == null)
			{
				return Unauthorized(new { Message = "Invalid username or password." });
			}

			var hasher = new PasswordHasher<User>();
			var result = hasher.VerifyHashedPassword(
				user,
				user.PasswordHash,
				dto.Password
			);

			if (result == PasswordVerificationResult.Failed)
			{
				return Unauthorized(new { Message = "Invalid username or password." });
			}

			var token = JwtTokenHeper.JwtTokenProvider(user, _config);

			return Ok(new { token });
		}
	}
}
