using DrinksHubAPI.DataAccess;
using DrinksHubAPI.DTOs;
using DrinksHubAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DrinksHubAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DrinksController : ControllerBase
	{
		private readonly IDrinksRepository _drinksRepository;

		public DrinksController(IDrinksRepository drinksRepository)
		{
			_drinksRepository = drinksRepository;
		}

		[HttpPost]
		public async Task<IActionResult> CreateDrink([FromBody] DrinkDto drinkDtoIn)
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

			return Ok(new { Message = $"{drinkDtoIn.Name} successfully added." });
		}

		[HttpGet]
		public async Task<IActionResult> GetAllDrinks()
		{
			List<Drink> drinks = await _drinksRepository.GetAllAsync();

			if (drinks == null || !drinks.Any())
			{
				return NotFound(new { Message = "No drinks found." });
			}

			List<DrinkDto> drinkDtos = new List<DrinkDto>();

			foreach(var drink in drinks)
			{
				drinkDtos.Add(new DrinkDto
				{
					Name = drink.Name,
					Description = drink.Description,
					Category = drink.Category,
					Type = drink.Type,
					ImageUrl = drink.ImageUrl
				});
			}

			return Ok(drinkDtos);
		}

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

			var drinkDto = new DrinkDto
			{
				Name = drink.Name,
				Description = drink.Description,
				Category = drink.Category,
				Type = drink.Type,
				ImageUrl = drink.ImageUrl
			};

			return Ok(drinkDto);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateDrink(int id, [FromBody] DrinkDto drinkDtoIn)
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

			return Ok(new { Message = $"Drink {drinkDtoIn.Name} successfully updated" });
		}

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
