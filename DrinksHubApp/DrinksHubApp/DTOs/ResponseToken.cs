using System.Text.Json.Serialization;

namespace DrinksHubApp.DTOs
{
	public class ResponseToken
	{
		[JsonPropertyName("tokenType")]
		public string TokenType { get; set; } = "Bearer";

		[JsonPropertyName("accessToken")]
		public string AccessToken { get; set; } = string.Empty;

		[JsonPropertyName("expiresIn")]
		public int ExpiresIn { get; set; }

		[JsonPropertyName("refreshToken")]
		public string RefreshToken { get; set; } = string.Empty;
	}
}

