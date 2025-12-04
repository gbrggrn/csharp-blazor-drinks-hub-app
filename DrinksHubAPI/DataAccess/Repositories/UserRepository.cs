using DrinksHubAPI.Data;
using DrinksHubAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace DrinksHubAPI.DataAccess.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly DrinksHubContext _context;

		public UserRepository(DrinksHubContext context)
		{
			_context = context;
		}

		public async Task AddAsync(User userIn)
		{
			await _context.Users.AddAsync(userIn);
		}

		public async Task DeleteAsync(int id)
		{
			await _context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
			await _context.SaveChangesAsync();
		}

		public IQueryable<User> GetAllQuery()
		{
			throw new NotImplementedException();
		}

		public async Task<User?> GetByIdAsync(int id)
		{
			return await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
		}

		public async Task<User?> GetByUsernameAsync(string username)
		{
			return await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
		}

		public Task UpdateAsync(int id, User userIn)
		{
			throw new NotImplementedException();
		}
	}
}
