using DrinksHubAPI.Model;

namespace DrinksHubAPI.DataAccess
{
	public class UserRepository : IUserRepository
	{
		public Task AddAsync(User userIn)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public IQueryable<User> GetAllQuery()
		{
			throw new NotImplementedException();
		}

		public Task<User?> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(int id, User userIn)
		{
			throw new NotImplementedException();
		}
	}
}
