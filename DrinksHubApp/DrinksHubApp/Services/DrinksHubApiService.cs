
using DrinksHubApp.DTOs;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace DrinksHubApp.Services
{
	public class DrinksHubApiService
	{
		HttpClient _http;

		public DrinksHubApiService(HttpClient http)
		{
			_http = http;
			_http.BaseAddress = new Uri("https://localhost:5119");
        }

		public async Task<List<DrinkDto>> GetAllDrinksAsync(List<DrinkQueryActions> actions, 
			DrinkSortOption? sortOption, 
			DrinkFilterOption? filterOption,
			DrinkFilterCategory? filterCategory,
			DrinkFilterType? filterType,
			String? searchParameter)
		{
			// If no actions, return all drinks or an empty list early
			if (actions == null || actions.Count == 0)
			{
				return await _http.GetFromJsonAsync<List<DrinkDto>>("api/Drinks") ?? new List<DrinkDto>();
			}

			StringBuilder request = new();

			for (int i = 0; i < actions.Count; i++)
			{
				request.Append(i == 0 ? "?" : "&");

				if (actions[i] == DrinkQueryActions.Search && !string.IsNullOrEmpty(searchParameter))
				{
					request.Append($"{DrinkQueryMapping.MapActions(actions[i])}={Uri.EscapeDataString(searchParameter)}");
				}
				else if (actions[i] == DrinkQueryActions.Sort && sortOption.HasValue)
				{
					request.Append($"{DrinkQueryMapping.MapActions(actions[i])}={DrinkQueryMapping.MapSortOptions(sortOption)}");
				}
				else if (actions[i] == DrinkQueryActions.Filter && filterOption.HasValue)
				{
					request.Append($"{DrinkQueryMapping.MapActions(actions[i])}={DrinkQueryMapping.MapFilterOptions(filterOption)}");
					if (filterCategory != null)
					{
						request.Append($"&{DrinkQueryMapping.MapFilterCategory(filterCategory)}");
					}
					else if (filterType != null)
					{
						request.Append($"&{DrinkQueryMapping.MapFilterType(filterType)}");
					}
				}
			}

			String requestString = request.ToString();

			var drinks = await _http.GetFromJsonAsync<List<DrinkDto>>($"api/Drinks{requestString}");

			//If no drinks found, return empty list
			return drinks ?? new List<DrinkDto>();
		}

		public async Task<DrinkDto?> GetDrinkByIdAsync(int idIn)
		{
			var drink = await _http.GetFromJsonAsync<DrinkDto>($"api/Drinks/{idIn}");
			return drink;
		}

		public async Task<bool> CreateDrinkAsync(DrinkDto drinkDtoIn)
		{
			var response = await _http.PostAsJsonAsync("api/Drinks", drinkDtoIn);

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> UpdateDrinkAsync(int idIn, DrinkDto drinkDtoIn)
		{
			var response = await _http.PutAsJsonAsync($"api/Drinks/{idIn}", drinkDtoIn);

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> DeleteDrinksAsync(int idIn)
		{
			var response = await _http.DeleteAsync($"api/Drinks/{idIn}");

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}
    }
}