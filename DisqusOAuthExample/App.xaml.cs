using System.Threading.Tasks;
using Xamarin.Forms;

namespace DisqusOAuthExample
{
	public partial class App : Application
	{
		private static DisqusClient _dsqClient;
		private static DisqusOAuthExampleViewModel _viewModel;

		public App()
		{
			InitializeComponent();

			MainPage = new DisqusOAuthExamplePage();
		}

		public static DisqusClient DsqClient
		{
			get
			{
				return _dsqClient;
			}
			set
			{
				_dsqClient = value;
			}
		}

		public static DisqusOAuthExampleViewModel ViewModel
		{
			get
			{
				if (_viewModel == null)
					_viewModel = new DisqusOAuthExampleViewModel();

				return _viewModel;
			}
		}

		protected override async void OnStart()
		{
			// Handle when your app starts

			await CheckDisqusUserStatusAsync();
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override async void OnResume()
		{
			// Handle when your app resumes

			await CheckDisqusUserStatusAsync();
		}

		private async Task CheckDisqusUserStatusAsync()
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				ViewModel.IsLoading = true;

				ViewModel.IsAuthenticated = await App.DsqClient.CheckAuthorizationAsync();
				if (ViewModel.IsAuthenticated)
				{
					await ViewModel.FetchUserAsync();
				}

				ViewModel.IsLoading = false;
			});
		}
	}
}
