
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

			BindingContext = viewModel;
		}
		protected override void OnAppearing()
		{
			viewModel.GetAppsCommand?.Execute(this);
			base.OnAppearing();
		}
	}
}