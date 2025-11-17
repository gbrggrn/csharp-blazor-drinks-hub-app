using System.ComponentModel.DataAnnotations;

namespace DrinksHubAPI.Model
{
	public class User
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Username { get; set; } = string.Empty;

		[Required]
		[MaxLength(50)]
		public string Password { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
	}
}
