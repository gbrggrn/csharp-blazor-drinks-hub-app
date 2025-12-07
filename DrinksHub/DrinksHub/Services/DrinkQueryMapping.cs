namespace DrinksHub.Services
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

		public static string MapFilterCategory(DrinkCategory? category) => category switch
		{
			DrinkCategory.Alcoholic => "alcoholic",
			DrinkCategory.NonAlcoholic => "nonAlcoholic",
			_ => throw new NotImplementedException()
		};

		public static string MapFilterType(DrinkType? type) => type switch
		{
			DrinkType.Cocktail => "cocktail",
			DrinkType.Mocktail => "mocktail",
			DrinkType.Smoothie => "smoothie",
			DrinkType.Juice => "juice",
			DrinkType.Soda => "soda",
			DrinkType.Wine => "wine",
			DrinkType.Beer => "beer",
			DrinkType.Tea => "tea",
			DrinkType.Coffee => "coffee",
			DrinkType.Other => "other",
			_ => throw new NotImplementedException()
		};
	}
}
