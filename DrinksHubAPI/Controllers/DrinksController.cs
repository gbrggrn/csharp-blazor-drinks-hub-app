using DrinksHubAPI.DataAccess;
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
		public async Task<IActionResult> CreateDrink(Drink drinkIn)
		{
			if (drinkIn == null)
			{
				return BadRequest(new { Message = "Invalid drink data provided." });
			}

			await _drinksRepository.AddAsync(drinkIn);

			return Ok(new { Message = "This is a placeholder for adding a drink." });
		}

		[HttpGet]
		public async Task<IActionResult> GetAllDrinks()
		{
			List<Drink> drinks = await _drinksRepository.GetAllAsync();

			if (drinks == null || !drinks.Any())
			{
				return NotFound(new { Message = "No drinks found." });
			}

			return Ok(drinks);
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

			return Ok(drink);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateDrink(int id, Drink drinkIn)
		{
			if (drinkIn == null)
			{
				return BadRequest(new { Message = "Invalid drink data provided." });
			}

			await _drinksRepository.UpdateAsync(id, drinkIn);

			return Ok(new { Message = $"Drink {drinkIn.Name} successfully updated" });
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
