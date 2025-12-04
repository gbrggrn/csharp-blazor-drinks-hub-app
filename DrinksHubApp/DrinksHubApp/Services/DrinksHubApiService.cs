
using DrinksHubApp.DTOs;
using System.Net.Http.Headers;
using System.Text;

namespace DrinksHubApp.Services
{
	public class DrinksHubApiService
	{
		private readonly HttpClient _http;
		private readonly TokenStore _tokenStore;

		public DrinksHubApiService(HttpClient http, TokenStore tokenStore)
		{
			_http = http;
			_tokenStore = tokenStore;
		}

		private void AttachTokenToRequest()
		{
			if (!string.IsNullOrEmpty(_tokenStore.Token))
			{
				_http.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", _tokenStore.Token);
			}
		}

		public async Task<List<ResponseDrinkDTO>> GetAllDrinksAsync(List<DrinkQueryActions> actions, 
			DrinkSortOption? sortOption, 
			DrinkFilterOption? filterOption,
			DrinkFilterCategory? filterCategory,
			DrinkFilterType? filterType,
			String? searchParameter)
		{
			// If no actions, return all drinks or an empty list early
			if (actions == null || actions.Count == 0)
			{
				return await _http.GetFromJsonAsync<List<ResponseDrinkDTO>>("api/Drinks") ?? new List<ResponseDrinkDTO>();
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

			var drinks = await _http.GetFromJsonAsync<List<ResponseDrinkDTO>>($"api/Drinks{requestString}");

			//If no drinks found, return empty list
			return drinks ?? new List<ResponseDrinkDTO>();
		}

		public async Task<ResponseDrinkDTO?> GetDrinkByIdAsync(int idIn)
		{
			var drink = await _http.GetFromJsonAsync<ResponseDrinkDTO>($"api/Drinks/{idIn}");
			return drink;
		}

		public async Task<bool> CreateDrinkAsync(CreateDrinkDTO drinkDtoIn)
		{
			var response = await _http.PostAsJsonAsync("api/Drinks", drinkDtoIn);

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> UpdateDrinkAsync(int idIn, UpdateDrinkDTO drinkDtoIn)
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

		public async Task<List<ResponseDrinkDTO>> GetFavoritesAsync(int userId)
		{
			var favorites = await _http.GetFromJsonAsync<List<ResponseDrinkDTO>>($"api/Favorites/{userId}");
				
			return favorites ?? new List<ResponseDrinkDTO>();
		}

		public async Task<bool> AddToFavoritesAsync(int drinkId, int userId)
		{
			var response = await _http.PostAsync($"api/Favorites/{drinkId}?userId={userId}", null);

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> RemoveFromFavoritesAsync(int drinkId, int userId)
		{
			var response = await _http.DeleteAsync($"api/Favorites/{drinkId}?userId={userId}");

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}
    }
}