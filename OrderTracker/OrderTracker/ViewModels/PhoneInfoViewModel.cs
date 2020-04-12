using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OrderTracker.Views;
using SQLite;
using Xamarin.Forms;

namespace OrderTracker
{
	public class PhoneInfoViewModel : ViewModelBase<PhoneInformation>
	{
		public PhoneInfoViewModel(Page page, bool isEditPage = false) : base(page)
		{
			if (AppList == null)
			{
				AppList = new ObservableCollection<AppNameTemplate>();
			}
			if (Operators == null)
			{
				Operators = new ObservableCollection<Operator>();
			}
			SavePhoneInfoCommand = new Command(async () => await SavePhoneInfo());
			SearchCommand = new Command(async () => await PushAsync<SearchPhoneInfo>());
			this.isEditPage = isEditPage;
		}

		public bool isEditPage;

		private ObservableCollection<AppNameTemplate> appList;

		public ObservableCollection<AppNameTemplate> AppList
		{
			get => appList;
			set => SetProperty(ref appList, value, nameof(AppList));
		}

		public ObservableCollection<Operator> operators;

		public ObservableCollection<Operator> Operators
		{
			get => operators;
			set => SetProperty(ref operators, value, nameof(Operators));
		}

		public Operator selectedOperator;

		public Operator SelectedOperator
		{
			get => selectedOperator;
			set
			{
				bool isSet = SetProperty(ref selectedOperator, value, nameof(SelectedOperator));
				if (isSet)
					Model.Company = value.Name;
			}
		}

		public async Task UpdateLists()
		{
			await RunAsync(async (ct) =>
			{
				List<AppName> data = await App.DbService.SelectAsync<AppName>();
				if (data.Any())
				{
					AppList.Clear();
					if (isEditPage)
					{
						var existingLinks = await App.DbService.SelectAsync<PhoneAppLink>(x => x.PhoneInfoId == Model.PhoneInfoId);
						foreach (var link in data)
						{
							link.IsSelected = existingLinks.Exists(x => x.AppNameId == link.AppNameId);
						}
					}
					AppList.AddRange(ToTemplate(data.OrderBy(x => x.Id).ToList()));
				}

				Operators.Clear();
				Operators.AddRange(new Operator[] {
				new Operator {Name = "Airtel", Id =1 },
				new Operator {Name = "Vodafone",Id =2 },
				new Operator {Name = "Reliance-Jio",Id =3 },
				new Operator {Name = "Idea",Id =4 },
				new Operator {Name = "Tata Docomo", Id =5 } });

				if (isEditPage)
				{
					SelectedOperator = Operators.FirstOrDefault(x => x.Name == Model.Company);
				}
			}, true);
		}

		public ICommand SavePhoneInfoCommand { get; set; }

		private async Task SavePhoneInfo()
		{
			await RunAsync(async (ct) =>
			{
				if (string.IsNullOrWhiteSpace(Model.PhoneNo) || Model.ExpiryDate == null || string.IsNullOrWhiteSpace(Model.Company))
				{
					LoggerService.LogError(new Exception("All required information is not provided"));
					return;
				}

				if (!isEditPage && await App.DbService.ExistsAsync<PhoneInformation>(x => x.PhoneNo == Model.PhoneNo))
				{
					LoggerService.LogError(new Exception($"Phone Number {Model.PhoneNo} already exists!"));
					return;
				}
				if (AppList.Count(x => x.IsSelected) == 0)
				{
					LoggerService.LogError(new Exception("Please select at-least one app."));
					return;
				}

				bool isCompleted = false;

				await App.DbService.Transaction((trans) =>
				{
					isCompleted = isEditPage ? UpdateData(trans) : InsertData(trans);
					return isCompleted;
				});

				if (!isEditPage && isCompleted)
				{
					var option = await BindingPage.DisplayAlert(Constants.SAVED_MSG, Constants.SAVE_OTHER_MSG, "ADD MORE", "GO BACK");
					if (option)
					{
						ResetModel();
						AppList.ForEach((a) =>
						{
							a.IsSelected = false;
						});
					}
					else
					{
						await PopAsync();
					}
				}
				else
				{
					await PopAsync();
				}
			});
		}

		public ICommand SearchCommand { get; private set; }

		private bool InsertData(SQLiteConnection trans)
		{
			bool isCompleted = false;
			var phoneInfoAdded = trans.Insert(Model);
			if (phoneInfoAdded == 1)
			{
				var phoneInfo = trans.Get<PhoneInformation>(x => x.PhoneNo == Model.PhoneNo);
				isCompleted = InsertAppLinks(phoneInfo, trans);
			}
			return isCompleted;
		}

		private bool UpdateData(SQLiteConnection trans)
		{
			bool isCompleted = false;
			var phoneInfoUpdated = trans.Update(Model);
			if (phoneInfoUpdated == 1)
			{
				var phoneInfo = trans.Get<PhoneInformation>(x => x.PhoneInfoId == Model.PhoneInfoId);
				if (phoneInfo.PhoneInfoId == Model.PhoneInfoId)
				{
					var existingLinks = trans.QueryTable<PhoneAppLink, int>(t => t.PhoneInfoId == phoneInfo.PhoneInfoId, t => t.PhoneAppLinkId);
					if (existingLinks.Any())
					{
						foreach (var record in existingLinks)
						{
							trans.Delete<PhoneAppLink>(record.PhoneAppLinkId);
						}
					}

					isCompleted = InsertAppLinks(phoneInfo, trans);
				}
			}
			return isCompleted;
		}

		private List<AppNameTemplate> ToTemplate(List<AppName> apps)
		{
			var temp = new List<AppNameTemplate>();
			int batchCount = 3;
			for (int i = 0; i < apps.Count; i += batchCount)
			{
				var count = batchCount;
				if (i + batchCount > apps.Count - 1)
					count = apps.Count - i;

				var currentApps = apps.GetRange(i, count).ToArray();
				temp.Add(new AppNameTemplate(currentApps));
			}

			return temp;
		}

		private bool InsertAppLinks(PhoneInformation phoneInfo, SQLiteConnection trans)
		{
			bool isCompleted = false;
			if (phoneInfo.PhoneInfoId > 0)
			{
				List<PhoneAppLink> appLinks = new List<PhoneAppLink>();
				var selectedApps = AppList.Where(x => x.IsSelected);
				foreach (var selectedTemp in selectedApps)
				{
					var selectedApp = selectedTemp.ToAppName();
					foreach (var app in selectedApp)
					{
						if (app.IsSelected)
							appLinks.Add(new PhoneAppLink { AppNameId = app.AppNameId, PhoneInfoId = phoneInfo.PhoneInfoId, PhoneNo = phoneInfo.PhoneNo });
					}
				}

				var inserted = trans.InsertAll(appLinks, true);
				if (inserted == appLinks.Count)
				{
					isCompleted = true;
				}
			}
			return isCompleted;
		}
	}
}