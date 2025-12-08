using DrinksHub.DTOs;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace DrinksHub.Services
{
	public class AuthService
	{
		private readonly HttpClient _http;
		private readonly TokenStore _tokenStore;

		public AuthService(HttpClient http, TokenStore tokenStore)
		{
			_http = http;
			_tokenStore = tokenStore;
		}

		public async Task<bool> LoginAsync(LoginRequestDTO loginRequestDTO)
		{
			if (loginRequestDTO == null)
			{
				return false;
			}

			var response = await _http.PostAsJsonAsync("api/Auth/login", loginRequestDTO);

			var raw = string.Empty;
			try
			{
				raw = await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"LoginAsync: failed to read response body: {ex.Message}");
			}

			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine("LoginAsync: response not successful");
				return false;
			}

			ResponseToken? tokenResponse = null;
			try
			{
				tokenResponse = await response.Content.ReadFromJsonAsync<ResponseToken>();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"LoginAsync: deserialization failed: {ex.Message}");
			}

			if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
			{
				Console.WriteLine("LoginAsync: tokenResponse is null or AccessToken missing");
				return false;
			}

			_tokenStore.SetToken(tokenResponse.AccessToken);

			return true;
		}

		public void Logout()
		{
			_tokenStore.ClearToken();
		}
	}
}
