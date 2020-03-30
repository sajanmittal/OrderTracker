
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
	public class OrderViewModel : ViewModelBase<Order>
	{

		public OrderViewModel(Page page, bool isEditPage = false) : base(page)
		{
			SaveCommand = new Command(async () => await SaveOrder());
			GetSearchDataCommand = new Command<SearchItem>(async (data) => await GetSearchData(data));
			ItemTapped = new Command<Order>(async (order) => await UpdateStatus(order));
			GenerateReportCommad = new Command(async () => await DownloadReport());
			this.isEditPage = isEditPage;
			if (OrderList == null)
				OrderList = new ObservableCollection<Order>();

		}

		public readonly bool isEditPage;

		public ICommand SaveCommand { get; set; }

		private async Task SaveOrder()
		{
			await RunAsync(async () =>
			{
				if (string.IsNullOrWhiteSpace(Model.PhoneNo) || Model.OrderDate == null || string.IsNullOrWhiteSpace(Model.CloneNo))
				{
					LoggerService.LogError(new Exception("All required information is not provided"));
					return;
				}

				if (isEditPage)
						await UpdateOrder();
					else
						await AddNewOrder();

			});
		}

		private ObservableCollection<Order> items;
		public ObservableCollection<Order> OrderList
		{
			get => items;
			set => SetProperty(ref items, value, nameof(OrderList));
		}

		public ICommand GetSearchDataCommand { get; set; }

		private async Task GetSearchData(SearchItem searchItem)
		{
			await RunAsync(async () =>
			{

				List<Order> result = new List<Order>();

				var query = (await App.DbService.SelectAsync<Order>(x => x.Status == Enums.OrderStatus.Pending)).AsQueryable();

				if (!string.IsNullOrWhiteSpace(searchItem.CloneNo))
				{
					query = query.Where(x => x.CloneNo != null && x.CloneNo.Contains(searchItem.CloneNo));
				}

				if (!string.IsNullOrWhiteSpace(searchItem.PhoneNo))
				{
					query = query.Where(x => x.PhoneNo != null && x.PhoneNo.Contains(searchItem.PhoneNo));
				}

				if (!string.IsNullOrWhiteSpace(searchItem.TrackingNo))
				{
					query = query.Where(x => x.TrackingNo != null && x.TrackingNo.Contains(searchItem.TrackingNo));
				}

				result.AddRange(query.OrderBy(x => x.OrderDate));

				if (result.Any())
				{
					OrderList.Clear();
					OrderList.AddRange(result);
					if (result.Count == 1)
						LoggerService.LogInformation($"{result.Count} Record Found");
					else
						LoggerService.LogInformation($"{result.Count} Records Found");
				}
				else
				{
					LoggerService.LogInformation("No Record Found");
				}
			});

		}

		public ICommand ItemTapped { get; set; }

		private async Task UpdateStatus(Order item)
		{
			await RunAsync(async () =>
			{
				Model = item;
				var updateStatus = await BindingPage.DisplayActionSheet($"Update {item.TrackingNo}({item.ShortDetail})", "Cancel", null, "Received", "Cancelled", "Update Record");

				switch (updateStatus)
				{
					case "Cancel":
						return;
					case "Received":
						item.Status = Enums.OrderStatus.Received;
						await UpdateItemStatus(item);
						break;
					case "Cancelled":
						item.Status = Enums.OrderStatus.Cancelled;
						await UpdateItemStatus(item);
						break;
					case "Update Record":
						{
							await PushAsync(new AddOrder(item));
							break;
						}
				}
			});
		}

		public ICommand GenerateReportCommad { get; set; }

		private async Task DownloadReport()
		{
			ReportGenResponse response = ReportGenResponse.Empty;
			var fileName = $"Order Summary {DateTime.Now.ToString(Constants.FILE_DATE_FRMT)}";
			await RunAsync(async () =>
			{
				IReportService reportService = new LocalReportService();
				var data = await App.DbService.SelectAsync<Order>();
				response = await reportService.Generate(data.OrderBy(x => x.Status), fileName);
			},
			(ex) =>
			{
				response = ReportGenResponse.Empty;
			});

			if(response.IsGenerated)
			{
				LoggerService.LogInformation($"Report {fileName} Successfully Dowloaded ");
			}

		}

		private async Task AddNewOrder()
		{
			Model.Status = Enums.OrderStatus.Pending;
			int records = await App.DbService.InsertAsync(Model);
			if (records > 0)
			{
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
		}

		private async Task UpdateOrder()
		{
			int updatedRecord = await App.DbService.UpdateAsync(Model);
			if(updatedRecord > 0)
			{
				await PopAsync();
			}
		}

		private async Task UpdateItemStatus(Order item)
		{
			var isUpdated = await App.DbService.UpdateAsync(item);
			if (isUpdated > 0)
			{
				var removedItem = items.FirstOrDefault(x => x.Id == item.Id);
				if (removedItem != null)
				{
						items.Remove(removedItem);
						LoggerService.LogInformation($"Order {removedItem.Status}.");
				}
			}
		}
	}
}
