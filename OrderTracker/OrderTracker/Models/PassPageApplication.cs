using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace OrderTracker
{
	public class PassPageApplication<T> : PassApplication where T : Page, new()
	{
		public PassPageApplication(MainViewModel viewModel)
		{
			Command = new Command(async () => await viewModel.PushAsync(new T()));
		}

		public override ICommand Command { get; set; }

		public override Type PageType =>  typeof(T);
	}
}
