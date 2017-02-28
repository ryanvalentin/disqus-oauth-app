using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DisqusOAuthExample.Droid
{
	[Activity(Label = "DisqusOAuthExample.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	[IntentFilter(new[] { Intent.ActionMain }, Categories = new[] { Intent.CategoryLauncher, Intent.CategorySampleCode }, Icon = "@drawable/icon", DataScheme = "disqusoauthexample", DataHost = "com.disqusoauthexample.disqus_oauth_example")]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App());
		}

		protected override void OnNewIntent(Intent intent)
		{
			base.OnNewIntent(intent);

			var data = intent.Data;

			System.Diagnostics.Debug.WriteLine("OnNewIntent");
		}
	}
}
