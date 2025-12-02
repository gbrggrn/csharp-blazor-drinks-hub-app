using DrinksHubAPI.Data;
using DrinksHubAPI.DTOs;
using DrinksHubAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DrinksHubAPI.DataAccess.Repositories
{
	public class FavoritesRepository : IFavoritesRepository
	{
		private readonly DrinksHubContext _context;

		public FavoritesRepository(DrinksHubContext context)
		{
			_context = context;
		}

		public async Task AddFavoriteToUserAsync(int userId, int drinkId)
		{
			var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
			var drinkExists = await _context.Drinks.AnyAsync(d => d.Id == drinkId);

			if (!userExists || !drinkExists)
			{
				throw new KeyNotFoundException("User or Drink not found.");
			}

			var favorite = new UserFavorite
			{
				UserId = userId,
				DrinkId = drinkId
			};

			_context.UserFavorites.Add(favorite);
			await _context.SaveChangesAsync();
		}

		public async Task<List<ResponseDrinkDTO>> GetFavoritesOfUser(int userId)
		{
			return await _context.UserFavorites
				.Where(f => f.UserId == userId)
				.Select(f => new ResponseDrinkDTO
				{
					Id = f.Drink.Id,
					Name = f.Drink.Name,
					Description = f.Drink.Description,
					Category = f.Drink.Category,
					Type = f.Drink.Type,
					ImageUrl = f.Drink.ImageUrl,
					Reviews = f.Drink.Reviews.Select(r => new ResponseReviewDTO
					{
						Id = r.Id,
						Title = r.Title,
						Content = r.Content,
						Rating = r.Rating,
						Username = r.User != null ? r.User.Username : "Unknown"
					}).ToList()
				}).ToListAsync();
		}

		public async Task RemoveFavoriteAsync(int userId, int drinkId)
		{
			var favorite = await _context.UserFavorites.FirstOrDefaultAsync(uf => uf.UserId == userId && uf.DrinkId == drinkId);

			if (favorite == null)
			{
				return;
			}
			
			_context.UserFavorites.Remove(favorite);
			await _context.SaveChangesAsync();
		}
	}
}
