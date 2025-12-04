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

		[Authorize(Roles = "Admin")]
		[HttpDelete("/api/users/{id}")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			if (id < 0)
			{
				return NotFound();
			}

			await _userRepository.DeleteAsync(id);

			return Ok();
		}

		[Authorize(Roles = "Admin")]
		[HttpPost("addUser")]
		public async Task<IActionResult> AddUser(CreateUserDTO createUserDTO)
		{
			if (createUserDTO == null)
			{
				return BadRequest("No user data provided.");
			}

			if (await _userRepository.GetByUsernameAsync(createUserDTO.Username) != null)
			{
				return BadRequest("Username already exists.");
			}

			var newUser = new User
			{
				Name = createUserDTO.Name,
				Email = createUserDTO.Email,
				Username = createUserDTO.Username,
				Role = createUserDTO.Role
			};

			var hasher = new PasswordHasher<User>();

			newUser.PasswordHash = hasher.HashPassword(newUser, createUserDTO.Password);

			await _userRepository.AddAsync(newUser);

			return Ok(new { Message = $"User: {newUser.Username} created."});
		}

		//Add update user
		//Add remove user
		//Add retrieve all users
	}
}
