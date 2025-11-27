namespace DrinksHubApp.Services
{
	public class DrinkQueryMapping
	{
		public static string MapActions(DrinkQueryActions action) => action switch
		{
			DrinkQueryActions.Search => "search",
			DrinkQueryActions.Sort => "sortBy",
			DrinkQueryActions.Filter => "filter",
			_ => throw new NotImplementedException(),
		};

		public static string MapSortOptions(DrinkSortOption? option) => option switch
		{
			DrinkSortOption.NameDesc => "nameDesc",
			DrinkSortOption.NameAsc => "nameAsc",
			DrinkSortOption.CategoryDesc => "categoryDesc",
			DrinkSortOption.CategoryAsc => "categoryAsc",
			DrinkSortOption.TypeDesc => "typeDesc",
			DrinkSortOption.TypeAsc => "typeAsc",
			_ => throw new NotImplementedException(),
		};

		public static string MapFilterOptions(DrinkFilterOption? option) => option switch
		{
			DrinkFilterOption.Category => "category",
			DrinkFilterOption.Type => "type",
			_ => throw new NotImplementedException()
		};

		public static string MapFilterCategory(DrinkFilterCategory? category) => category switch
		{
			DrinkFilterCategory.Alcoholic => "alcoholic",
			DrinkFilterCategory.NonAlcoholic => "nonAlcoholic",
			_ => throw new NotImplementedException()
		};

		public static string MapFilterType(DrinkFilterType? type) => type switch
		{
			DrinkFilterType.Cocktail => "cocktail",
			DrinkFilterType.Mocktail => "mocktail",
			DrinkFilterType.Smoothie => "smoothie",
			DrinkFilterType.Juice => "juice",
			DrinkFilterType.Soda => "soda",
			DrinkFilterType.Wine => "wine",
			DrinkFilterType.Beer => "beer",
			DrinkFilterType.Tea => "tea",
			DrinkFilterType.Coffee => "coffee",
			DrinkFilterType.Other => "other",
			_ => throw new NotImplementedException()
		};
	}
}
