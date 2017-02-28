using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DisqusOAuthExample
{
	public partial class DisqusOAuthExamplePage : ContentPage
	{
		public DisqusOAuthExampleViewModel ViewModel => (DisqusOAuthExampleViewModel)BindingContext;

		public DisqusOAuthExamplePage()
		{
			InitializeComponent();

			BindingContext = App.ViewModel;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		private void Login_Clicked(object sender, System.EventArgs e)
		{
			ViewModel.Login();
		}

		private void Logout_Clicked(object sender, System.EventArgs e)
		{
			ViewModel.Logout();
		}
	}
}
