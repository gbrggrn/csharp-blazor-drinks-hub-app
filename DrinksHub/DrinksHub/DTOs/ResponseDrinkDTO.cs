using DrinksHub.Services;

namespace DrinksHub.DTOs
{
	public class ResponseDrinkDTO
	{
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DrinkCategory Category { get; set; }
        public DrinkType Type { get; set; }
        public string ImageUrl { get; set; } = "";
    }
}
