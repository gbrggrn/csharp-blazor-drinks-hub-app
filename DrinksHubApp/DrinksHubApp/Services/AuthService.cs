using DrinksHubApp.DTOs;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace DrinksHubApp.Services
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
				Console.WriteLine("LoginAsync: loginRequestDTO is null");
				return false;
			}

			var response = await _http.PostAsJsonAsync("api/Auth/login", loginRequestDTO);

			// Debug: log status & raw body
			var raw = string.Empty;
			try
			{
				raw = await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"LoginAsync: failed to read response body: {ex.Message}");
			}
			Console.WriteLine($"LoginAsync: status={response.StatusCode}, rawBody={raw}");

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

			Console.WriteLine($"LoginAsync: token deserialized, setting token (len={tokenResponse.AccessToken.Length})");
			_tokenStore.SetToken(tokenResponse.AccessToken);

			return true;
		}
	}
}
