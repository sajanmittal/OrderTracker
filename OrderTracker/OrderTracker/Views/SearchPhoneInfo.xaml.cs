using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrderTracker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchPhoneInfo : ContentPage
	{
		private SearchPhoneViewModel viewModel;

		public SearchPhoneInfo(bool isExipryData = false)
		{
			InitializeComponent();
			if (viewModel == null)
				viewModel = new SearchPhoneViewModel(this, isExipryData);

			BindingContext = viewModel;
		}

		protected async override void OnAppearing()
		{
			await viewModel.SetPhoneInfo();
			base.OnAppearing();
		}

		private void PhoneSearchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(e.NewTextValue))
				PhoneListView.ItemsSource = viewModel.PhoneInfoList;
			else
				PhoneListView.ItemsSource = viewModel.PhoneInfoList.Where(x => x.PhoneNo.Contains(e.NewTextValue));
		}

		private void PhoneListView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var phoneInfo = e.Item as PhoneInformation;
			if (viewModel != null)
			{
				viewModel.ItemTapped?.Execute(phoneInfo);
			}
		}
	}
}