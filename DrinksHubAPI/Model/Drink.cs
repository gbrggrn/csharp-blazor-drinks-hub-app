using System.ComponentModel.DataAnnotations;

namespace DrinksHubAPI.Data
{
	public class Drink
	{
		public int id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[Required]
		[MaxLength(500)]
		public string Description { get; set; } = string.Empty;

		[Required]
		[MaxLength(50)]
		public string Category { get; set; } = string.Empty;

		[Required]
		[MaxLength(50)]
		public string Type { get; set; } = string.Empty;
		public string ImageUrl { get; set; } = string.Empty;
	}
}
