using System;
using Plugin.Settings;
using Xamarin.Forms;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using System.Net.Http;

namespace DisqusOAuthExample
{
	public class DisqusClient
	{
		private const string _oauthBaseUrl = "https://disqus.com/api/oauth/2.0/authorize/";
		private const string _apiBaseUrl = "https://disqus.com/api/3.0/";
		private const string _loginBaseUrl = "https://disqus.com/profile/login/";

		private readonly HttpClient _client;

		private OAuthResponse _authorization { get; set; } 
		private Uri _activationUrl { get; set; }
		private string _apiKey { get; set; }

		public DisqusClient(string apiKey)
		{
			_apiKey = apiKey;
			_client = new HttpClient();

			string authString = CrossSettings.Current.GetValueOrDefault<string>("DisqusAuthorization");

			if (!String.IsNullOrEmpty(authString))
				_authorization = JsonConvert.DeserializeObject<OAuthResponse>(authString);
		}

		public bool IsAuthenticated
		{
			get
			{
				return !String.IsNullOrEmpty(_authorization?.AccessToken);
			}
		}

		private string GetParam(IEnumerable<KeyValuePair<string, string>> queryParams, string key)
		{
			return Uri.UnescapeDataString(queryParams.FirstOrDefault(q => String.Compare(q.Key, key, true) == 0).Value);
		}

		public void StartAuthorizeUser()
		{
			if (IsAuthenticated)
				return;

			// Generate a random state and store it. We'll compare it to the incoming URL request later
			// to see if it originated from this request.
			string state = Guid.NewGuid().ToString("N");
			CrossSettings.Current.AddOrUpdateValue("DisqusAuthState", state);

			string oauthUrl = $"{_oauthBaseUrl}?client_id={_apiKey}&scope=read,write,email&response_type=code&state={state}";

			// Route OAuth URLs through the Disqus login page first, because it allows users to log in with Twitter,
			// Facebook, etc., while the OAuth dialog doesn't.
			Uri fullUri = new Uri($"{_loginBaseUrl}?next={Uri.EscapeDataString(oauthUrl)}");

			Device.OpenUri(fullUri);
		}

		public void QueueAuthorizationUrl(string activationUrlString)
		{
			Uri activationUrl;
			if (Uri.TryCreate(activationUrlString, UriKind.Absolute, out activationUrl))
			{
				_activationUrl = activationUrl;
			}
		}

		/// <summary>
		/// Checks the authorization status, and logs the user in if the app was activated from a protocol response.
		/// </summary>
		/// <returns>Whether the user is authenticated.</returns>
		public async Task<bool> CheckAuthorizationAsync()
		{
			if (_activationUrl != null)
			{
				var queryParams = _activationUrl
					.Query
					.Substring(1, _activationUrl.Query.Length - 1)
					.Split('&')
					.Select(kvp =>
					{
						var splits = kvp.Split('=');
						return new KeyValuePair<string, string>(splits[0], splits[1]);
					});
				var payloadString = GetParam(queryParams, "payload");
				var state = GetParam(queryParams, "state");

				// Make sure the state is what we've stored earlier
				string storedState = CrossSettings.Current.GetValueOrDefault<string>("DisqusAuthState");
				if (!String.IsNullOrEmpty(storedState) && !String.IsNullOrEmpty(payloadString) && storedState == state)
				{
					try
					{
						CrossSettings.Current.Remove("DisqusAuthState");
						_activationUrl = null;

						CrossSettings.Current.AddOrUpdateValue("DisqusAuthorization", payloadString);
						_authorization = JsonConvert.DeserializeObject<OAuthResponse>(payloadString);
					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine(ex.Message);
					}
				}
			}

			if (IsAuthenticated && _authorization.Expires < DateTime.UtcNow.AddDays(2))
			{
				// Refresh the authentication if the token will expire in 2 days or less.
				await RefreshAuthorizationAsync();
			}

			return IsAuthenticated;
		}

		public async Task RefreshAuthorizationAsync()
		{
			if (!IsAuthenticated)
			{
				StartAuthorizeUser();
				return;
			}

			// TODO: Set up a function endpoint that takes a refresh token, and refreshes the authorization using
			// a stored secret key. It should return the refreshed authentication. If authorization fails, be sure to
			// delete the authentication so the application doesn't think the user is logged in.
		}

		public void Logout()
		{
			CrossSettings.Current.Remove("DisqusAuthorization");
			_authorization = null;
		}

		public async Task<User> GetAuthenticatedUserDetailsAsync()
		{
			string content = await _client.GetStringAsync($"{_apiBaseUrl}users/details.json?api_key={_apiKey}&access_token={_authorization.AccessToken}");

			return JsonConvert.DeserializeObject<ApiResponse<User>>(content).Response;
		}

		public class OAuthResponse
		{
			private long _expiresIn;

			[JsonProperty(PropertyName = "expires")]
			public DateTime Expires { get; set; }

			[JsonProperty(PropertyName = "username")]
			public string Username { get; set; }

			[JsonProperty(PropertyName = "user_id")]
			public long UserId { get; set; }

			[JsonProperty(PropertyName = "access_token")]
			public string AccessToken { get; set; }

			[JsonProperty(PropertyName = "expires_in")]
			public long ExpiresIn
			{ 
				get { return _expiresIn; }
				set
				{
					_expiresIn = value;
					Expires = DateTime.UtcNow.AddSeconds(_expiresIn);
				}
			}

			[JsonProperty(PropertyName = "token_type")]
			public string TokenType { get; set; }

			[JsonProperty(PropertyName = "state")]
			public string State { get; set; }

			[JsonProperty(PropertyName = "scope")]
			public string Scope { get; set; }

			[JsonProperty(PropertyName = "refresh_token")]
			public string RefreshToken { get; set; }
		}

		public class ApiResponse<T>
		{
			[JsonProperty(PropertyName = "code")]
			public int? Code { get; set; }

			[JsonProperty(PropertyName = "response")]
			public T Response { get; set; }

			[JsonProperty(PropertyName = "cursor")]
			public object Cursor { get; set; }
		}

		public class User
		{
			[JsonProperty(PropertyName = "name")]
			public string Name { get; set; }

			[JsonProperty(PropertyName = "username")]
			public string Username { get; set; }

			[JsonProperty(PropertyName = "email")]
			public string Email { get; set; }

			[JsonProperty(PropertyName = "isVerified")]
			public bool? IsVerified { get; set; }
		}
	}
}
