using Newtonsoft.Json;
using System;

namespace DisqusOAuthExample.Services.Disqus.Models
{
	public class DsqOAuthResponse
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
}
