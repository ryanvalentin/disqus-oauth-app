using System;
using System.Net.Http;

namespace DisqusOAuthExample.Services.Disqus
{
	public sealed partial class DisqusService : IDisposable
	{
		private const string _oauthBaseUrl = "https://disqus.com/api/oauth/2.0/authorize/";
		private const string _apiBaseUrl = "https://disqus.com/api/3.0/";
		private const string _loginBaseUrl = "https://disqus.com/profile/login/";

		private readonly HttpClient _httpClient;

		public DisqusService()
		{
			
		}
	}
}
