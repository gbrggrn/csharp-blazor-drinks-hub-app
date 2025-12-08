using DrinksHubAPI.DataAccess;
using DrinksHubAPI.DTOs;
using DrinksHubAPI.Helpers;
using DrinksHubAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
			Console.WriteLine($"Login attempt: Username='{dto?.Username}', Password='{dto?.Password}'");

			if (dto == null)
			{
				return BadRequest("No login data provided.");
			}

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
				return Unauthorized(new { Message = "Invalid either username or password." });
			}

			var jwt = JwtTokenHelper.JwtTokenProvider(user, _config);

			var response = new ResponseToken
			{
				AccessToken = jwt,
				ExpiresIn = 3600,
				RefreshToken = Guid.NewGuid().ToString()
			};

			Console.WriteLine($"{response} returned");
			return Ok(response);
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

		//Not implemented yet
		[Authorize(Roles = "Admin")]
		[HttpPut("updateUser")]
		public async Task<IActionResult> UpdateUser(int id, [FromQuery] UpdateUserDTO updateUserDTO)
		{
			if (updateUserDTO == null)
			{
				return BadRequest("No user data provided.");
			}

			if (id <= 0)
			{
				return BadRequest("No user with provided ID.");
			}

			var updatedUser = new User
			{
				Name = updateUserDTO.Name,
				Email = updateUserDTO.Email,
				Username = updateUserDTO.Username,
				Role = updateUserDTO.Role
			};

			var hasher = new PasswordHasher<User>();

			updatedUser.PasswordHash = hasher.HashPassword(updatedUser, updateUserDTO.Password);

			await _userRepository.UpdateAsync(id, updatedUser);

			return Ok(new { Message = $"User: {updatedUser.Username} updated."});
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> GetUsers()
		{
			var allUsers = await _userRepository.GetAllQuery().ToListAsync();

			return Ok(allUsers);
		}
	}
}
