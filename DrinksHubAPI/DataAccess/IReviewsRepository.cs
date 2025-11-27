using DrinksHubAPI.Model;

namespace DrinksHubAPI.DataAccess
{
	public interface IReviewsRepository
	{
		Task AddAsync(Review review);
		Task DeleteAsync(Review review);
	}
}
