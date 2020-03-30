using OrderTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OrderTracker
{
	public class PhoneInfoViewModel : ViewModelBase<PhoneInformation>
	{
		public PhoneInfoViewModel(Page page) : base(page)
		{
			if (AppList == null)
			{
				AppList = new ObservableCollection<AppName>();
			}
			SavePhoneInfoCommand = new Command(async () => await SavePhoneInfo());
			SearchCommand = new Command(async () => await PushAsync<SearchPhoneInfo>());
		}

		private ObservableCollection<AppName> appList;
		public ObservableCollection<AppName> AppList
		{
			get => appList;
			set => SetProperty(ref appList, value, nameof(AppList));
		}

		
		public async Task UpdateAppList()
		{
		await RunAsync(async () =>  { 
			List<AppName> data = await App.DbService.SelectAsync<AppName>();
			if (data.Any())
			{
				AppList.Clear();
				AppList.AddRange(data.OrderBy(x => x.Id));
			}
			});
		}

		public ICommand SavePhoneInfoCommand { get; set; }

		private async Task SavePhoneInfo()
		{
			await RunAsync(async () =>
			{
				if (string.IsNullOrWhiteSpace(Model.PhoneNo) || Model.ExpiryDate == null || string.IsNullOrWhiteSpace(Model.Company))
				{
					LoggerService.LogError(new Exception("All required information is not provided"));
					return;
				}

				if (await App.DbService.ExistsAsync<PhoneInformation>(x => x.PhoneNo == Model.PhoneNo))
				{
					LoggerService.LogError(new Exception($"Phone Number {Model.PhoneNo} already exists!"));
					return;
				}
				if (AppList.Count(x => x.IsSelected) == 0)
				{
					LoggerService.LogError(new Exception("Please select atleast one app."));
					return;
				}


				bool isCompleted = false;

				await App.DbService.Transaction((trans) =>
				{
					var phoneInfoAdded = trans.Insert(Model);
					if (phoneInfoAdded == 1)
					{
						var phoneInfo = trans.Get<PhoneInformation>(x => x.PhoneNo == Model.PhoneNo);
						if (phoneInfo.PhoneInfoId > 0)
						{
							List<PhoneAppLink> appLinks = new List<PhoneAppLink>();
							var selectedApps = AppList.Where(x => x.IsSelected);
							foreach (var selectedApp in selectedApps)
							{
								appLinks.Add(new PhoneAppLink { AppNameId = selectedApp.AppNameId, PhoneInfoId = phoneInfo.PhoneInfoId, PhoneNo = phoneInfo.PhoneNo });
							}

							var inserted = trans.InsertAll(appLinks, true);
							if (inserted == appLinks.Count)
							{
								isCompleted = true;
							}
						}
					}
					return isCompleted;
				});

				if(isCompleted)
					{
					var option = await BindingPage.DisplayAlert(Constants.SAVED_MSG, Constants.SAVE_OTHER_MSG, "ADD MORE", "GO BACK");
					if (option)
					{
						ResetModel();
						AppList.ForEach( (a) => {
							a.IsSelected = false;
						});
					}
					else
					{
						await PopAsync();
					}
				}
			});
		}

		public ICommand SearchCommand { get; private set; }

	}
}
