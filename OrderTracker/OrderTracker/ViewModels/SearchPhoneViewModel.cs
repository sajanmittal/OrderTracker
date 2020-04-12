using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OrderTracker.Views;
using SQLite;
using Xamarin.Forms;

namespace OrderTracker
{
	public class SearchPhoneViewModel : ViewModelBase<PhoneInformation>
	{
		public SearchPhoneViewModel(Page page) : base(page)
		{
			if (PhoneInfoList == null)
			{
				PhoneInfoList = new ObservableCollection<PhoneInformation>();
			}
			GenerateReportCommad = new Command(async () => await DownloadReport());
			ItemTapped = new Command<PhoneInformation>(async (item) => await ItemTap(item));
		}

		private ObservableCollection<PhoneInformation> phoneInfoList;

		public ObservableCollection<PhoneInformation> PhoneInfoList
		{
			get => phoneInfoList;
			set => SetProperty(ref phoneInfoList, value, nameof(PhoneInfoList));
		}

		public async Task SetPhoneInfo()
		{
			var data = await App.DbService.SelectAsync<PhoneInformation>();
			if (data.Any())
			{
				PhoneInfoList.Clear();
				PhoneInfoList.AddRange(data);
			}
		}

		public ICommand GenerateReportCommad { get; set; }

		private async Task DownloadReport()
		{
			ReportGenResponse response = ReportGenResponse.Empty;
			var fileName = $"Phone Numbers {DateTime.Now.ToString(Constants.FILE_DATE_FRMT)}";
			await RunAsync(async (ct) =>
			{
				IReportService reportService = new LocalReportService();
				var phoneInfo = (await App.DbService.SelectAsync<PhoneInformation>()).AsQueryable();
				var appInfo = (await App.DbService.SelectAsync<AppName>()).AsQueryable();
				var phoneAppLink = (await App.DbService.SelectAsync<PhoneAppLink>()).AsQueryable();

				var groupedData = (from pa in phoneAppLink
								   join ai in appInfo on pa.AppNameId equals ai.AppNameId
								   join p in phoneInfo on pa.PhoneInfoId equals p.PhoneInfoId
								   group new { p.PhoneNo, p.Company, p.SimNo, ai.Name } by ai.Name into newGroup
								   select newGroup).ToList();

				response = await reportService.Generate(groupedData, fileName);
			},
			true,
			(ex) =>
			{
				response = ReportGenResponse.Empty;
			});

			if (response.IsGenerated)
			{
				LoggerService.LogInformation($"Report {fileName} Successfully Dowloaded ");
			}
		}

		public ICommand ItemTapped { get; private set; }

		private async Task ItemTap(PhoneInformation item)
		{
			await RunAsync(async (ct) =>
			{
				Model = item;
				var updateStatus = await BindingPage.DisplayActionSheet($"Update {item.PhoneNo}({item.Company})", "Cancel", null, "Update Record", "Delete Record");

				switch (updateStatus)
				{
					case "Cancel":
						return;

					case "Update Record":
						await PushAsync(new AddPhoneInfo(item));
						break;

					case "Delete Record":
						await Delete();
						break;
				}
			});
		}

		private async Task Delete()
		{
			var option = await BindingPage.DisplayAlert($"Delete {Model.PhoneNo}", "Are you sure, you want to DELETE?", "Yes", "Cancel");
			if (option)
			{
				bool isDeleted = false;
				if (Model?.PhoneInfoId <= 0)
				{
					throw new ArgumentNullException("No Item is selected to delete!!");
				}
				await App.DbService.Transaction((trans) =>
				{
					isDeleted = DeleteData(trans);
					return isDeleted;
				});

				if (isDeleted)
				{
					await SetPhoneInfo();
				}
			}
		}

		private bool DeleteData(SQLiteConnection trans)
		{
			bool isCompleted = false;
			var phoneInfo = trans.Get<PhoneInformation>(x => x.PhoneInfoId == Model.PhoneInfoId);
			if (phoneInfo.PhoneInfoId == Model.PhoneInfoId)
			{
				var existingLinks = trans.QueryTable<PhoneAppLink, int>(t => t.PhoneInfoId == phoneInfo.PhoneInfoId, t => t.PhoneAppLinkId);
				if (existingLinks.Any())
				{
					foreach (var record in existingLinks)
					{
						trans.Delete<PhoneAppLink>(record);
					}
				}

				int isdeleted = trans.Delete<PhoneInformation>(phoneInfo);
				if (isdeleted == 1)
				{
					isCompleted = true;
				}
			}
			return isCompleted;
		}
	}
}