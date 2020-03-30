using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrderTracker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddOrder : ContentPage
	{
		OrderViewModel viewModel;

		public AddOrder()
		{
			InitializeComponent();
			SetBindingContext(false);
		}

		public AddOrder(Order model)
		{
			InitializeComponent();
			SetBindingContext(true);
			viewModel.Model = model;
		}

		private void SetBindingContext(bool isEditPage)
		{
			if (viewModel == null)
				viewModel = new OrderViewModel(this, isEditPage);

			BindingContext = viewModel;
		}


		protected override void OnAppearing()
		{
			SearchButton.IsEnabled = !viewModel.isEditPage;
			base.OnAppearing();
		}

		private async void Search_Clicked(object sender, EventArgs e)
		{
			await viewModel.PushAsync<SearchOrder>();
		}
	}
}