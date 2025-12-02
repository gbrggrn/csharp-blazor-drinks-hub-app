using DrinksHubAPI.DataAccess;
using DrinksHubAPI.DTOs;
using DrinksHubAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DrinksHubAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DrinksController : ControllerBase
	{
		private readonly IDrinksRepository _drinksRepository;
		private readonly IUserRepository _userRepository;
		private readonly IReviewsRepository _reviewsRepository;

		public DrinksController(IDrinksRepository drinksRepository, IUserRepository userRepository, IReviewsRepository reviewsRepository)
		{
			_drinksRepository = drinksRepository;
			_userRepository = userRepository;
			_reviewsRepository = reviewsRepository;
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> CreateDrink([FromBody] CreateDrinkDTO drinkDtoIn)
		{
			if (drinkDtoIn == null)
			{
				return BadRequest(new { Message = "Invalid drink data provided." });
			}

			var drink = new Drink {
				Name = drinkDtoIn.Name,
				Description = drinkDtoIn.Description,
				Category = drinkDtoIn.Category,
				Type = drinkDtoIn.Type,
				ImageUrl = drinkDtoIn.ImageUrl
			};

			await _drinksRepository.AddAsync(drink);

			return Ok(new { Message = $"{drink.Name} successfully added." });
		}

		[Authorize]
		[HttpPost("{drinkId}/reviews")]
		public async Task<IActionResult> AddReviewToDrink (
			int drinkId, 
			[FromBody] CreateReviewDTO reviewDtoIn)
		{
			var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (!int.TryParse(userIdString, out int userId))
			{
				return Unauthorized(new { Message = "User is not authenticated" });
			}

			if (reviewDtoIn == null)
			{
				return BadRequest(new { Message = "The review DTO was null" });
			}

			var drink = await _drinksRepository.GetByIdAsync(drinkId);
			if (drink == null)
			{
				return NotFound(new { Message = "Drink not found" });
			}

			var review = new Review
			{
				Title = reviewDtoIn.Title,
				Content = reviewDtoIn.Content,
				Rating = reviewDtoIn.Rating,
				DrinkId = drinkId,
				UserId = userId
			};

			await _reviewsRepository.AddAsync(review);

			return Ok(new { Message = $"Review: {review.Title} - added to drink: {drink.Name}" });
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetAllDrinks(
			[FromQuery] string? search,
			[FromQuery] string? sortBy,
			[FromQuery] string? filter,
			[FromQuery] string? filterCategory,
			[FromQuery] string? filterType)
		{
			var drinks = _drinksRepository.GetAllQuery();

			// Apply search
			if (!string.IsNullOrEmpty(search))
			{
				drinks = drinks.Where(d =>
					d.Name.Contains(search) ||
					d.Description.Contains(search) ||
					d.Category.Contains(search) ||
					d.Type.Contains(search));
			}

			// Apply sorting
			if (!string.IsNullOrEmpty(sortBy))
			{
				drinks = sortBy.ToLower() switch
				{
					"nameDesc" => drinks.OrderByDescending(d => d.Name),
					"nameAsc" => drinks.OrderBy(d => d.Name),
					"categoryDesc" => drinks.OrderByDescending(d => d.Category),
					"categoryAsc" => drinks.OrderBy(d => d.Category),
					"typeDesc" => drinks.OrderByDescending(d => d.Type),
					"typeAsc" => drinks.OrderBy(d => d.Type),
					_ => drinks // No sorting or invalid sorting parameter
				};
			}

			// Apply filtering
			if (!string.IsNullOrEmpty(filterCategory))
			{
				drinks = drinks.Where(d => d.Category.Equals(filterCategory, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrEmpty(filterType))
			{
				drinks = drinks.Where(d => d.Type.Equals(filterType, StringComparison.OrdinalIgnoreCase));
			}

			var drinkDtos = await drinks.Select(drinks => new ResponseDrinkDTO
			{
				Name = drinks.Name,
				Description = drinks.Description,
				Category = drinks.Category,
				Type = drinks.Type,
				ImageUrl = drinks.ImageUrl
			}).ToListAsync();

			return Ok(drinkDtos);
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetDrinkById(int id)
		{
			if (id <= 0)
			{
				return BadRequest(new { Message = "Invalid drink ID provided." });
			}

			Drink? drink = await _drinksRepository.GetByIdAsync(id);

			if (drink == null)
			{
				return NotFound(new { Message = $"Drink with ID: {id} not found." });
			}

			var drinkDto = new ResponseDrinkDTO
			{
				Name = drink.Name,
				Description = drink.Description,
				Category = drink.Category,
				Type = drink.Type,
				ImageUrl = drink.ImageUrl,
				Reviews = drink.Reviews.Select(r => new ResponseReviewDTO
				{
					Title = r.Title,
					Content = r.Content,
					Rating = r.Rating,
					Username = r.User != null ? r.User.Username : "Unknown"
				}).ToList()
			};

			return Ok(drinkDto);
		}

		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateDrink(
			int id, 
			[FromBody] UpdateDrinkDTO drinkDtoIn)
		{
			if (drinkDtoIn == null)
			{
				return BadRequest(new { Message = "Invalid drink data provided." });
			}

			var drink = new Drink
			{
				Id = id,
				Name = drinkDtoIn.Name,
				Description = drinkDtoIn.Description,
				Category = drinkDtoIn.Category,
				Type = drinkDtoIn.Type,
				ImageUrl = drinkDtoIn.ImageUrl
			};

			try
			{
				await _drinksRepository.UpdateAsync(id, drink);
			} 
			catch (KeyNotFoundException kfe)
			{
				return NotFound(new { Message = kfe.Message });
			}

			return Ok(new { Message = $"Drink {drink.Name} successfully updated" });
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteDrink(int id)
		{
			if (id <= 0)
			{
				return BadRequest(new { Message = "Invalid drink ID prodvided." });
			}

			await _drinksRepository.DeleteAsync(id);

			return Ok(new { Message = $"Drink with id: {id} successfully removed." });
		}
	}
}
