namespace DrinksHubAPI.Model
{
	public class UserFavorite
	{
		public int Id { get; set; }
		
		public int UserId { get; set; }
		public User User { get; set; }

		public int DrinkId { get; set; }
		public Drink Drink { get; set; }
	}
}
