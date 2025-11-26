using DrinksHubAPI.DTOs;
using DrinksHubAPI.Model;

namespace DrinksHubAPI.DataAccess
{
	public interface IDrinksRepository
	{
		Task<List<Drink>> GetAllAsync();
		Task<Drink?> GetByIdAsync(int id);
		Task AddAsync(Drink drinkIn);
		Task UpdateAsync(int id, Drink drinkIn);
		Task DeleteAsync(int id);
	}
}