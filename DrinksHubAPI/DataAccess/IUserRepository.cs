using DrinksHubAPI.Model;

namespace DrinksHubAPI.DataAccess
{
	public interface IUserRepository
	{
		IQueryable<User> GetAllQuery();
		Task<User?> GetByIdAsync(int id);
		Task AddAsync(User userIn);
		Task UpdateAsync(int id, User userIn);
		Task DeleteAsync(int id);
	}
}
