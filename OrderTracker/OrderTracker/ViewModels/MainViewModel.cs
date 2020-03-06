using OrderTracker.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OrderTracker
{
    public class MainViewModel : BaseViewModel<SearchItem>
    {
        public MainViewModel(INavigation navigation) : base(navigation)
        {
            SearchCommand = new Command(async () => await SearchAsync());
            AddOrderCommand = new Command(async () => await AddOrder());
        }

        public ICommand SearchCommand { get; private set; }

        public ICommand AddOrderCommand { get; private set; }

        public async Task SearchAsync()
        {
            await RunAsync(async () => { await Navigation.PushAsync(new OrderList(Model)); });
        }

        public async Task AddOrder()
        {
            await RunAsync(async () =>
            {
                await Navigation.PushAsync(new AddOrder());
            });
        }
    }
}
