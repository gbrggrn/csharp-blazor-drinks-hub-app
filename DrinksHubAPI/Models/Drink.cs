using DrinksHubAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace DrinksHubAPI.Model
{
	public class Drink
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;

		[Required]
		[MaxLength(500)]
		public string Description { get; set; } = string.Empty;

		[Required]
		public DrinkCategory Category { get; set; }

		[Required]
		public DrinkType Type { get; set; }
		public string ImageUrl { get; set; } = string.Empty;

		public List<Review> Reviews { get; set; } = new();
	}
}