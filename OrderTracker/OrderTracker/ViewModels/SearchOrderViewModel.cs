using OrderTracker.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OrderTracker
{
	public class SearchOrderViewModel : ViewModelBase<SearchItem>
	{
		public SearchOrderViewModel(Page page) : base(page)
		{
			SearchCommand = new Command(async () => await SearchAsync());
		}

		public ICommand SearchCommand { get; private set; }

		private async Task SearchAsync()
		{
			await RunAsync(async () => { 
			await PushAsync(new OrderList(Model)); 
			});
		}
	}
}
