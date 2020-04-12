using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrderTracker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPhoneInfo : ContentPage
	{
		private PhoneInfoViewModel viewModel;

		public AddPhoneInfo()
		{
			InitializeComponent();
			if (viewModel == null)
			{
				viewModel = new PhoneInfoViewModel(this);
			}
			BindingContext = viewModel;
		}

		public AddPhoneInfo(PhoneInformation Model)
		{
			InitializeComponent();
			if (viewModel == null)
			{
				viewModel = new PhoneInfoViewModel(this, true);
			}
			BindingContext = viewModel;
			viewModel.Model = Model;
		}

		protected async override void OnAppearing()
		{
			await viewModel.UpdateLists();
			SearchButton.IsEnabled = !viewModel.isEditPage;
			PhoneEntry.IsReadOnly = viewModel.isEditPage;
			base.OnAppearing();
		}
	}
}