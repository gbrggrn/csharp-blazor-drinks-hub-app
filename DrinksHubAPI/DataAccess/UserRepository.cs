using DrinksHubAPI.Data;
using DrinksHubAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace DrinksHubAPI.DataAccess
{
	public class UserRepository : IUserRepository
	{
		private readonly DrinksHubContext _context;

		public UserRepository(DrinksHubContext context)
		{
			_context = context;
		}

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

		public async Task<User?> GetByIdAsync(int id)
		{
			return await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
		}

		public Task UpdateAsync(int id, User userIn)
		{
			throw new NotImplementedException();
		}
	}
}
