using DrinksHubAPI.Models;

namespace DrinksHubAPI.DTOs
{
	public class CreateDrinkDTO
	{
        public int Id { get; set; }
		public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DrinkCategory Category { get; set; }
        public DrinkType Type { get; set; }
        public string ImageUrl { get; set; } = "";
        public List<ResponseReviewDTO> Reviews { get; set; } = new();
	}
}
