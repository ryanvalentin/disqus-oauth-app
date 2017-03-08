using Newtonsoft.Json;
using System;

namespace DisqusOAuthExample.Services.Disqus.Models
{
	public class DsqApiResponse<T>
	{
		[JsonProperty(PropertyName = "code")]
		public int? Code { get; set; }

		[JsonProperty(PropertyName = "response")]
		public T Response { get; set; }

		[JsonProperty(PropertyName = "cursor")]
		public object Cursor { get; set; }
	}
}
