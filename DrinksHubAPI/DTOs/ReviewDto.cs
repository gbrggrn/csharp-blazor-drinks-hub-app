namespace DrinksHubAPI.DTOs
{
	public class ReviewDto
	{
		public int Id { get; set; }
		public string Title { get; set; } = "";
		public string Content { get; set; } = "";
		public int Rating { get; set; }
		public string Username { get; set; } = "";
	}
}