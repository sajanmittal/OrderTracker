using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrderTracker
{
	public abstract class ViewModelBase<T> : BaseViewModel where T : IBaseModel, new()
	{
		public ViewModelBase( Page page)
		{
			BindingPage = page;
			if (model == null)
				ResetModel();
		}

		private T model;

		public T Model
		{
			get => model;
			set => SetProperty(ref model, value, nameof(Model));
		}

		private Page page;

		public Page BindingPage
		{
			get => page;
			set => SetProperty(ref page, value, nameof(BindingPage));
		}

		protected void ResetModel()
		{
			Model = new T();
		}

		public async Task PushAsync<P>() where P : Page, new()
		{
			await PushAsync(new P());
		}

		public async Task PushAsync(Page page)
		{
				await BindingPage.Navigation.PushAsync(page);
		}

		public async Task PopAsync()
		{
				await BindingPage.Navigation.PopAsync();
		}

		public DateTime Today => DateTime.Today;
	}
}
