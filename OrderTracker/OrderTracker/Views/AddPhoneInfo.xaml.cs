using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			if(viewModel == null)
			{
				viewModel = new PhoneInfoViewModel(this);
			}
			BindingContext = viewModel;
		}

		protected async override void OnAppearing()
		{
			await viewModel.UpdateAppList();
			base.OnAppearing();
		}
	}
}