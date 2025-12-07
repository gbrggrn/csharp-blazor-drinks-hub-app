namespace DrinksHubAPI.Models
{
	public enum DrinkSortOption
	{
		NameAsc,
		NameDesc,
		CategoryAsc,
		CategoryDesc,
		TypeAsc,
		TypeDesc
	}

	public enum DrinkFilterOption
	{
		Category,
		Type
	}

	public enum DrinkCategory
	{
		Alcoholic,
		NonAlcoholic
	}

	public enum DrinkType
	{
		Cocktail,
		Mocktail,
		Smoothie,
		Juice,
		Soda,
		Wine,
		Beer,
		Tea,
		Coffee,
		Other
	}
}
