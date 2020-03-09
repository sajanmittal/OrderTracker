
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OrderTracker
{
    public class OrderViewModel : BaseViewModel<Order>
    {

        public OrderViewModel(INavigation navigation, Page page) : base(navigation, page)
        {
            SaveCommand = new Command(async () => await SaveOrder());
            GetSearchDataCommand = new Command<SearchItem>(async (data) => await GetSearchData(data));
            ItemTapped = new Command<Order>(async (order) => await UpdateStatus(order));
            if (OrderList == null)
                OrderList = new ObservableCollection<Order>();

        }

        public ICommand SaveCommand { get; set; }

        public async Task SaveOrder()
        {
            await RunAsync(async () =>
            {

                if (!string.IsNullOrWhiteSpace(Model.PhoneNo) && Model.OrderDate != null)
                {
                    Model.Status = OrderStatus.Pending;
                    int records = await App.DbService.InsertAsync(Model);
                    if (records > 0)
                    {
                        var option = await Page.DisplayAlert(Constants.SAVED_MSG, Constants.SAVE_OTHER_MSG, "SAVE ANOTHER", "GO BACK");
                        if (option)
                        {
                            ResetModel();
                        }
                        else
                        {
                            await Navigation.PopAsync();
                        }
                    }
                }
                else
                {
                    LoggerService.LogError(new Exception("All required information is not provided"));
                }
            });
        }

        private ObservableCollection<Order> items;
        public ObservableCollection<Order> OrderList
        {
            get
            {
                return items;
            }
            set
            {
                SetProperty(ref items, value, nameof(OrderList));
            }
        }

        public ICommand GetSearchDataCommand { get; set; }

        public async Task GetSearchData(SearchItem searchItem)
        {
            await RunAsync(async () =>
            {

                List<Order> result = new List<Order>();

                var query = (await App.DbService.SelectAsync<Order>(x => x.Status == OrderStatus.Pending)).AsQueryable();

                if(!string.IsNullOrWhiteSpace(searchItem.CloneNo))
                {
                    query = query.Where(x => x.CloneNo != null && x.CloneNo.Contains(searchItem.CloneNo));
                }

                if(!string.IsNullOrWhiteSpace(searchItem.PhoneNo))
                {
                    query = query.Where(x => x.PhoneNo != null && x.PhoneNo.Contains(searchItem.PhoneNo));
                }

                if(!string.IsNullOrWhiteSpace(searchItem.TrackingNo))
                {
                    query = query.Where(x => x.TrackingNo != null && x.TrackingNo.Contains(searchItem.TrackingNo));
                }

                result.AddRange(query.OrderBy(x => x.OrderDate));

                if (result.Count > 0)
                {
                    result.ForEach(x => OrderList.Add(x));
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

        public async Task UpdateStatus(Order item)
        {
            await RunAsync(async () =>
            {
                var updateStatus = await Page.DisplayActionSheet($"Update {item.TrackingNo}({item.ShortDetail})", "Cancel", null, "Received", "Cancelled", Constants.UPDATE_TRACKING_NO);

                switch (updateStatus)
                {
                    case "Cancel":
                        return;
                    case "Received":
                        item.Status = OrderStatus.Received;
                        break;
                    case "Cancelled":
                        item.Status = OrderStatus.Cancelled;
                        break;
                    case Constants.UPDATE_TRACKING_NO:
                        {
                            var newNo = await Page.DisplayPromptAsync(Constants.UPDATE_TRACKING_NO, $"Old Tracking No {item.TrackingNo}", "OK", "Cancel", null, 100, Keyboard.Numeric);
                            if (!string.IsNullOrEmpty(newNo))
                            {
                                item.TrackingNo = newNo;
                            }
                            break;
                        }

                }

                var isUpdated = await App.DbService.UpdateAsync(item);
                if (isUpdated > 0)
                {
                    var removedItem = items.FirstOrDefault(x => x.Id == item.Id);
                    if (removedItem != null)
                    {
                        if (updateStatus.Equals(Constants.UPDATE_TRACKING_NO))
                        {
                            int index = items.IndexOf(removedItem);
                            items[index] = item;
                            LoggerService.LogInformation($"Tracking Number Updated.");
                        }
                        else
                        {
                            items.Remove(removedItem);
                            LoggerService.LogInformation($"Order {updateStatus}.");
                        }
                    }
                }
            });
        }

    }
}
