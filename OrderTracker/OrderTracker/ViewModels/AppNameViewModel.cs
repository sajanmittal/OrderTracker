using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System;

namespace OrderTracker
{
	public class AppNameViewModel : ViewModelBase<AppName>
	{
		public AppNameViewModel( Page page) : base(page)
		{
			GetAppsCommand = new Command(async () => await GetAppData());
			AddAppNameCommand = new Command(async () => await AddAppName(), CanAddAppName);
			if (AppList == null)
				AppList = new ObservableCollection<AppName>();
		}

		private ObservableCollection<AppName> appList;
		public ObservableCollection<AppName> AppList
		{
			get => appList;
			set => SetProperty(ref appList, value, nameof(AppList));
		}

		public ICommand GetAppsCommand { get; set; }

		private async Task GetAppData()
		{
			await RunAsync(async () =>
			{
				List<AppName> data = await App.DbService.SelectAsync<AppName>();
				if (data.Any())
				{
					AppList.Clear();
					AppList.AddRange(data.OrderBy(x => x.Id));
				}
			});
		}

		public ICommand AddAppNameCommand{ get; set; }

		private bool CanAddAppName()
		{
			if (string.IsNullOrWhiteSpace(Model.Name))
			{
				LoggerService.LogError(new Exception("App Name is not provided"));
				return false;
			}

			if(appList.Any(x => x.Name.ToLower() == Model.Name.ToLower()))
			{
				LoggerService.LogError(new Exception($"App Name {Model.Name} already exists!"));
				return false;
			}

			return true;
		}

		private async Task AddAppName()
		{
			await RunAsync(async () =>
			{
				int records = await App.DbService.InsertAsync(Model);
				if (records > 0)
				{
					await GetAppData();
					var option = await Page.DisplayAlert(Constants.SAVED_MSG, Constants.SAVE_OTHER_MSG, "ADD MORE", "GO BACK");
					if (option)
					{
						ResetModel();
					}
					else
					{
						await Page.Navigation.PopAsync();
					}
				}
			});
		}
	}
}
