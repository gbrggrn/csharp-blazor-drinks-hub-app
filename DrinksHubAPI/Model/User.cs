using System.ComponentModel.DataAnnotations;

namespace DrinksHubAPI.Model
{
	public class User
	{
		public int Id { get; set; }

		[Required] 
		public string Name { get; set; } = string.Empty;

		[Required] 
		public string Email { get; set; } = string.Empty;


		[Required]
		[MaxLength(50)]
		public string Username { get; set; } = string.Empty;

		[Required]
		[MaxLength(50)]
		public string PasswordHash { get; set; } = string.Empty;
		public string Role { get; set; } = "User";
	}
}
