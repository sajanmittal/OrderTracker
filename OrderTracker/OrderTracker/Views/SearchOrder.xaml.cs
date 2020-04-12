using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrderTracker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchOrder : ContentPage
	{
		private SearchOrderViewModel viewModel;

		public SearchOrder()
		{
			InitializeComponent();
			if (viewModel == null)
				viewModel = new SearchOrderViewModel(this);

			BindingContext = viewModel;
		}
	}
}