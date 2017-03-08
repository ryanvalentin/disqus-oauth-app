using System;

namespace DisqusOAuthExample.Services.Disqus
{
	public class DisqusAuthentication
	{
		public DisqusAuthentication(string publicKey, string accessToken = "")
		{
			PublicKey = publicKey;
			AccessToken = accessToken;
		}

		public DisqusAuthentication(string publicKey, string secretKey, string remoteAuth)
			: this(publicKey)
		{
			SecretKey = secretKey;
			RemoteAuth = remoteAuth;
		}

		public readonly string PublicKey;

		public string SecretKey { get; set; }

		public string AccessToken { get; set; }

		public string RemoteAuth { get; set; }
	}
}
