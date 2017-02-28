using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace DisqusOAuthExample.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		private static Xamarin.Forms.Application _globalApp = null;

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			_globalApp = new App();
			LoadApplication(_globalApp);

			App.DsqClient = new DisqusClient("hDuMtiXLQn5TarhIlbB9Q8hpYYvDRS2QPa64U31QIi1DVu5pB4epANLFQeey4HIB");

			return base.FinishedLaunching(app, options);
		}

		public override void OnActivated(UIApplication uiApplication)
		{
			base.OnActivated(uiApplication);
		}

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			//disqusoauthexample://authorization/?payload=%7B%22username%22%3A%22ryanvalentin%22%2C%22user_id%22%3A375483%2C%22access_token%22%3A%22dadcc817baf94792b9ffd400f5a8f54d%22%2C%22expires_in%22%3A2592000%2C%22token_type%22%3A%22Bearer%22%2C%22state%22%3Anull%2C%22scope%22%3A%22read%2Cwrite%2Cemail%22%2C%22refresh_token%22%3A%22548010c1f0c4437aafa99204789caf4d%22%7D&state=cd6dd5cd25a348b1bc1ec2cb7977ed6a
			if (url.Host.Contains("authorization") && url.Scheme.Contains("disqusoauthexample"))
			{
				App.DsqClient.QueueAuthorizationUrl(url.AbsoluteString);
				return true;
			}
			return false;
		}
	}
}
