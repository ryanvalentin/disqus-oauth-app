using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DisqusOAuthExample
{
	public partial class DisqusOAuthExamplePage : ContentPage
	{
		DisqusOAuthExampleViewModel ViewModel => (DisqusOAuthExampleViewModel)BindingContext;

		public DisqusOAuthExamplePage()
		{
			InitializeComponent();

			BindingContext = App.ViewModel;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
		}

		void Handle_Clicked(object sender, System.EventArgs e)
		{
			App.DsqClient.StartAuthorizeUser();
		}
	}
}
