namespace DrinksHub.DTOs
{
	public class CreateReviewDTO
	{
		public int Id { get; set; }
		public string Title { get; set; } = "";
		public string Content { get; set; } = "";
		public int Rating { get; set; }
		public string Username { get; set; } = "";
	}
}
