using DrinksHubAPI.Model;

namespace DrinksHubAPI.DataAccess
{
	public interface IDrinksRepository
	{
		Task<List<Drink>> GetAllAsync();
		Task<Drink?> GetByIdAsync(int id);
		Task AddAsync(Drink drink);
		Task UpdateAsync(int id, Drink drink);
		Task DeleteAsync(int id);
	}
}