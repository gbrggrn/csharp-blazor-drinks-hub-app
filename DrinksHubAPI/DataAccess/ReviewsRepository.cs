using DrinksHubAPI.Data;
using DrinksHubAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace DrinksHubAPI.DataAccess
{
	public class ReviewsRepository : IReviewsRepository
	{
		private readonly DrinksHubContext _context;

		public ReviewsRepository(DrinksHubContext context)
		{
			_context = context;
		}

		public async Task AddAsync(Review review)
		{
			await _context.AddAsync(review);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			_context.Reviews.Where(r => r.Id == id).ExecuteDelete();
			await _context.SaveChangesAsync();
		}
	}
}
