using DrinksHubAPI.Data;
using DrinksHubAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace DrinksHubAPI.DataAccess
{
	public class DrinksRepository : IDrinksRepository
	{
		private readonly DrinksHubContext _context;

		public DrinksRepository(DrinksHubContext context)
		{
			_context = context;
		}

		public async Task AddAsync(Drink drink)
		{
			var newDrink = new Drink
			{
				Name = drink.Name,
				Description = drink.Description,
				Category = drink.Category,
				Type = drink.Type,
				ImageUrl = drink.ImageUrl
			};

			await _context.Drinks.AddAsync(newDrink);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var drink = await _context.Drinks.FindAsync(id);

			if (drink != null)
			{
				_context.Drinks.Remove(drink);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<List<Drink>> GetAllAsync()
		{
			return await _context.Drinks.ToListAsync();
		}

		public async Task<Drink?> GetByIdAsync(int id)
		{
			return await _context.Drinks.FindAsync(id);
		}

		public async Task UpdateAsync(int id, Drink drink)
		{
			var drinkUpdate = await _context.Drinks.Where(d => d.Id == id).FirstAsync();

			drinkUpdate.Name = drink.Name;
			drinkUpdate.Description = drink.Description;
			drinkUpdate.Category = drink.Category;
			drinkUpdate.Type = drink.Type;
			drinkUpdate.ImageUrl = drink.ImageUrl;

			_context.Drinks.Update(drinkUpdate);

			await _context.SaveChangesAsync();
		}
	}
}
