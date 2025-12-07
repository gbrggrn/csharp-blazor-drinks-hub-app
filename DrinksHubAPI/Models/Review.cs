using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinksHubAPI.Model
{
	public class Review
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Title { get; set; } = string.Empty;

		[Required]
		[MaxLength(1000)]
		public string Content { get; set; } = string.Empty;

		[Required]
		[Range(1, 5)]
		public int Rating { get; set; }

		// Foregin Key to Drink
		public Drink? Drink { get; set; }

		[Required]
		public int DrinkId { get; set; }

		// Foregin Key to User
		public User? User { get; set; }

		[Required]
		public int UserId { get; set; }
	}
}
