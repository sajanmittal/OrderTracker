
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OrderTracker
{
    public class OrderViewModel : BaseViewModel<Order>
    {
        public OrderViewModel(INavigation navigation) : base(navigation)
        {
            SaveCommand = new Command<Page>(async (page) => await SaveOrder(page));
            GetSearchDataCommand = new Command<SearchItem>(async (data) => await GetSearchData(data));
            if (OrderList == null)
                OrderList = new ObservableCollection<Order>();

        }

        public ICommand SaveCommand { get; set; }

        public async Task SaveOrder(Page page)
        {
            if(!string.IsNullOrWhiteSpace(Model.PhoneNo) && !string.IsNullOrWhiteSpace(Model.TrackingNo))
            {
                Model.Status = OrderStatus.Pending;
                int records = await App.DbService.InsertAsync(Model);
                if(records > 0)
                {
                    var option = await page.DisplayAlert(Constants.SAVED_MSG, Constants.SAVE_OTHER_MSG, "MORE", "DONE");
                    if(option)
                    {
                        ResetModel();
                    }
                    else
                    {
                       await Navigation.PopAsync();
                    }
                }
            }
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
            List<Order> result = new List<Order>();
            if(!string.IsNullOrWhiteSpace(searchItem.PhoneNo) && !string.IsNullOrWhiteSpace(searchItem.TrackingNo))
            {
                result.AddRange( await App.DbService.SelectAsync<Order>(x => x.TrackingNo.Contains(searchItem.TrackingNo) && x.PhoneNo.Contains(searchItem.PhoneNo)));
            }
            else if(!string.IsNullOrWhiteSpace(searchItem.TrackingNo))
            {
                result.AddRange(await App.DbService.SelectAsync<Order>(x => x.TrackingNo.Contains(searchItem.TrackingNo)));
            }
            else if (!string.IsNullOrWhiteSpace(searchItem.PhoneNo))
            {
                result.AddRange(await App.DbService.SelectAsync<Order>(x => x.TrackingNo.Contains(searchItem.PhoneNo)));
            }
            else
            {
                result.AddRange(await App.DbService.SelectAsync<Order>());
            }

            if(result.Count > 0)
            {
                result.ForEach(x => OrderList.Add(x)); 
            }
        }
    }
}
