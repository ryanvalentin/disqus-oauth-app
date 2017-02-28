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
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			App.DsqClient = new DisqusClient("hDuMtiXLQn5TarhIlbB9Q8hpYYvDRS2QPa64U31QIi1DVu5pB4epANLFQeey4HIB");

			return base.FinishedLaunching(app, options);
		}

		public override void OnActivated(UIApplication uiApplication)
		{
			base.OnActivated(uiApplication);
		}

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			if (url.Host.Contains("authorization") && url.Scheme.Contains("disqusoauthexample"))
			{
				App.DsqClient.QueueAuthorizationUrl(url.AbsoluteString);
				return true;
			}
			return false;
		}
	}
}
