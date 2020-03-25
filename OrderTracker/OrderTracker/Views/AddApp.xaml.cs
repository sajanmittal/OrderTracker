
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrderTracker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddApp : ContentPage
	{
		public AppNameViewModel viewModel;
		public AddApp()
		{
			InitializeComponent();
			if (viewModel == null)
				viewModel = new AppNameViewModel(this);
		}
		protected override void OnAppearing()
		{
			viewModel.GetAppsCommand?.Execute(null);
			base.OnAppearing();
		}
	}
}