using DrinksHubAPI.Data;
using DrinksHubAPI.DTOs;
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

		public async Task AddAsync(Drink drinkIn)
		{
			await _context.Drinks.AddAsync(drinkIn);
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

		public IQueryable<Drink> GetAllQuery()
		{
			return _context.Drinks.AsQueryable();
		}

		public async Task<Drink?> GetByIdAsync(int id)
		{
			// Iclude Reviews and associated Users
			Drink? drink = await _context.Drinks.Include(d => d.Reviews)
				.ThenInclude(r => r.User)
				.FirstOrDefaultAsync(d => d.Id == id);

			if (drink == null)
			{
				return null;
			}

			return drink;
		}

		public async Task UpdateAsync(int id, Drink drink)
		{
			Drink? drinkToUpdate = await _context.Drinks.Where(d => d.Id == id).FirstOrDefaultAsync();

			if (drinkToUpdate == null)
			{
				throw new KeyNotFoundException($"Drink with ID {id} not found.");
			}

			drinkToUpdate.Name = drink.Name;
			drinkToUpdate.Description = drink.Description;
			drinkToUpdate.Category = drink.Category;
			drinkToUpdate.Type = drink.Type;
			drinkToUpdate.ImageUrl = drink.ImageUrl;

			_context.Drinks.Update(drinkToUpdate);

			await _context.SaveChangesAsync();
		}
	}
}
