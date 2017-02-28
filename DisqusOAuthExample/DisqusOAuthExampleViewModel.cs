using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DisqusOAuthExample
{
	public class DisqusOAuthExampleViewModel : INotifyPropertyChanged
	{
		private string _username;
		private string _name;
		private bool _isLoading;
		private bool _isAuthenticated;

		public DisqusOAuthExampleViewModel()
		{
			
		}

		public string Username
		{
			get { return _username; }
			set
			{
				if (_username != value)
				{
					_username = value;
					RaisePropertyChanged(nameof(Username));
				}
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if (_name != value)
				{
					_name = value;
					RaisePropertyChanged(nameof(Name));
				}
			}
		}

		public bool IsLoading
		{
			get { return _isLoading; }
			set
			{
				if (_isLoading != value)
				{
					_isLoading = value;
					RaisePropertyChanged(nameof(IsLoading));
				}
			}
		}

		public bool IsAuthenticated
		{
			get { return _isAuthenticated; }
			set
			{
				if (_isAuthenticated != value)
				{
					_isAuthenticated = value;
					RaisePropertyChanged(nameof(IsAuthenticated));
					RaisePropertyChanged(nameof(ShowLogin));
				}
			}
		}

		public bool ShowLogin
		{
			get
			{
				return !IsAuthenticated;
			}
		}

		public void Login()
		{
			App.DsqClient.StartAuthorizeUser();
		}

		public async Task FetchUserAsync()
		{
			if (!IsAuthenticated)
				return;

			try
			{
				var user = await App.DsqClient.GetAuthenticatedUserDetailsAsync();

				Name = user.Name;
				Username = user.Username;
			}
			catch (Exception ex)
			{
				// TODO: Handle error
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		public void Logout()
		{
			App.DsqClient.Logout();
			IsAuthenticated = false;
			Name = "";
			Username = "";
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged(string propName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}
	}
}
