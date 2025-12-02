using DrinksHubAPI.DTOs;

namespace DrinksHubAPI.DataAccess
{
	public interface IFavoritesRepository
	{
		public Task AddFavoriteToUserAsync(int userId, int drinkId);
		public Task RemoveFavoriteAsync(int userId, int drinkId);
		public Task<List<ResponseDrinkDTO>> GetFavoritesOfUser(int userId);
	}
}
