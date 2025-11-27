namespace DrinksHubAPI.DTOs
{
	public class ResponseDrinkDTO
	{
        public int Id { get; set; }
		public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public string Type { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public List<ResponseReviewDTO> Reviews { get; set; } = new();
	}
}
