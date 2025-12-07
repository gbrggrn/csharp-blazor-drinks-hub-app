namespace DrinksHub.Services
{
	public enum DrinkQueryActions
	{
		Search,
		Sort,
		Filter,
		All
	}

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

	public enum DrinkFilterCategory
	{
		Alcoholic,
		NonAlcoholic
	}

	public enum DrinkFilterType
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
