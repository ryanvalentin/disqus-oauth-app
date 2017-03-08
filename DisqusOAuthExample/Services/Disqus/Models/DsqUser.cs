using Newtonsoft.Json;
using System;

namespace DisqusOAuthExample.Services.Disqus.Models
{
	public class DsqUser
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
