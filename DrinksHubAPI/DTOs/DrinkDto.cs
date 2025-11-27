namespace DrinksHubAPI.DTOs
{
	public class DrinkDto
	{
        public int Id { get; set; }
		public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public string Type { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public List<ReviewDto> Reviews { get; set; } = new();
	}
}
