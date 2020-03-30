using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OrderTracker
{
	public class SearchPhoneViewModel : ViewModelBase<EmptyModel>
	{
		public SearchPhoneViewModel(Page page): base(page)
		{
			if (PhoneInfoList == null)
			{
				PhoneInfoList = new ObservableCollection<PhoneInformation>();
			}
			GenerateReportCommad = new Command(async () => await DownloadReport());
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
			if(data.Any())
			{
				PhoneInfoList.Clear();
				PhoneInfoList.AddRange(data);
			}
		}

		public ICommand GenerateReportCommad{ get; set; }

		private async Task DownloadReport()
		{
			ReportGenResponse response = ReportGenResponse.Empty;
			var fileName = $"Phone Numbers {DateTime.Now.ToString(Constants.FILE_DATE_FRMT)}";
			await RunAsync(async () =>
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
			(ex) =>
			{
				response = ReportGenResponse.Empty;
			});

			if (response.IsGenerated)
			{
				LoggerService.LogInformation($"Report {fileName} Successfully Dowloaded ");
			}

		}
	}
}
