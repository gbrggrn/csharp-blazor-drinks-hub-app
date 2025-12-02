using DrinksHubAPI.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrinksHubAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FavoritesController : ControllerBase
	{
		private readonly IFavoritesRepository _favoritesRepository;
		public FavoritesController(IFavoritesRepository favoritesRepository)
		{
			_favoritesRepository = favoritesRepository;
		}

		[Authorize]
		[HttpGet("{userId}")]
		public async Task<IActionResult> GetFavorites(int userId)
		{
			var drinkDTOs = await _favoritesRepository.GetFavoritesOfUser(userId);

			return Ok(drinkDTOs);
		}

		[Authorize]
		[HttpPost("{drinkId}")]
		public async Task<IActionResult> AddToFavorites(int drinkId, [FromQuery] int userId)
		{
			await _favoritesRepository.AddFavoriteToUserAsync(drinkId, userId);

			return Ok(new { Message = "Drink added to favorites." });
		}

		[Authorize]
		[HttpDelete("{drinkId}")]
		public async Task<IActionResult> RemoveFromFavorites(int drinkId, [FromQuery] int userId)
		{
			await _favoritesRepository.RemoveFavoriteAsync(drinkId, userId);

			return Ok(new { Message = "Drink removed from favorites." });
		}
	}
}
