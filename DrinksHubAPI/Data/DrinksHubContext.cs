using Microsoft.EntityFrameworkCore;
using DrinksHubAPI.Model;

namespace DrinksHubAPI.Data
{

	public class DrinksHubContext : DbContext
	{
		public DrinksHubContext(DbContextOptions<DrinksHubContext> options) : base(options)
		{
		}

		public DbSet<Drink> Drinks { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserFavorite> UserFavorites { get; set; }
	}
}


