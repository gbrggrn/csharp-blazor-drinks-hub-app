using DrinksHubAPI.Data;
using DrinksHubAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			await _context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
			await _context.SaveChangesAsync();
		}

		public IQueryable<User> GetAllQuery()
		{
			return _context.Users.AsQueryable();
		}

		public async Task<User?> GetByIdAsync(int id)
		{
			return await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
		}

		public async Task<User?> GetByUsernameAsync(string username)
		{
			return await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
		}

		public async Task UpdateAsync(int id, User userIn)
		{
			User? userToUpdate = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

			if (userToUpdate == null)
			{
				throw new KeyNotFoundException($"Could not find user to update.");
			}

			userToUpdate.Name = userIn.Name;
			userToUpdate.Username = userIn.Username;
			userToUpdate.Role = userIn.Role;
			userToUpdate.Email = userIn.Email;
			userToUpdate.PasswordHash = userIn.PasswordHash;

			_context.Users.Update(userToUpdate);

			await _context.SaveChangesAsync();
		}
	}
}
