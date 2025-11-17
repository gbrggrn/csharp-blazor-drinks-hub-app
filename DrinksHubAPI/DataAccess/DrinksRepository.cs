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
			await _context.Drinks.AddAsync(drink);
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

		public async Task UpdateAsync(Drink drink)
		{
			_context.Drinks.Update(drink);
			await _context.SaveChangesAsync();
		}
	}
}
