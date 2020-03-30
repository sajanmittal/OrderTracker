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
			AddAppNameCommand = new Command(async () => await AddAppName());
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
				await UpdateAppList();
			});
		}

		public ICommand AddAppNameCommand{ get; set; }

		private async Task AddAppName()
		{
			await RunAsync(async () =>
			{

				if (string.IsNullOrWhiteSpace(Model.Name))
				{
					LoggerService.LogError(new Exception("App Name is not provided"));
					return;
				}

				if (appList.Any(x => x.Name.Trim().ToLower() == Model.Name.Trim().ToLower()))
				{
					LoggerService.LogError(new Exception($"App Name {Model.Name} already exists!"));
					return;
				}

				int records = await App.DbService.InsertAsync(Model);
				if (records > 0)
				{
					await UpdateAppList();
					var option = await BindingPage.DisplayAlert(Constants.SAVED_MSG, Constants.SAVE_OTHER_MSG, "ADD MORE", "GO BACK");
					if (option)
					{
						ResetModel();
					}
					else
					{
						await PopAsync();
					}
				}
			});
		}

		private async Task UpdateAppList()
		{
			List<AppName> data = await App.DbService.SelectAsync<AppName>();
			if (data.Any())
			{
				AppList.Clear();
				AppList.AddRange(data.OrderBy(x => x.Id));
			}
		}
	}
}
