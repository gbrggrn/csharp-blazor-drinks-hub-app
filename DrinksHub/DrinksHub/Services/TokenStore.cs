
namespace DrinksHub.Services
{
	public class TokenStore
	{
		private string? _token;
		public string? Token
		{
			get => _token;
			private set
			{
				if (_token != value)
				{
					_token = value;
					OnTokenChanged?.Invoke();
				}
			}
		}

		public event Action? OnTokenChanged;

		public void SetToken(string token)
		{
			Token = token;
		}

		public void ClearToken()
		{
			Token = null;
		}
	}
}
