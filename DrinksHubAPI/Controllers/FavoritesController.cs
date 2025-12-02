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
		[HttpGet]
		public Task<IActionResult> GetFavorites()
		{
			return Task.FromResult<IActionResult>(Ok("GetFavorites endpoint reached."));
		}
	}
}
