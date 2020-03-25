using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrderTracker
{
	public abstract class ViewModelBase<T> : BaseViewModel where T : BaseModel, new()
	{
		public ViewModelBase( Page page)
		{
			Page = page;
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

		public Page Page
		{
			get => page;
			set => SetProperty(ref page, value, nameof(Page));
		}

		protected void ResetModel()
		{
			Model = new T();
		}

		public async Task RunAsync(Func<Task> action, Action<Exception> catchHandler = null, Action finalHadler = null)
		{
			try
			{
				if (!IsBusy)
				{
					IsBusy = true;
					await action?.Invoke();
				}
			}
			catch (Exception ex)
			{
				LoggerService.LogError(ex);
				catchHandler?.Invoke(ex);
			}
			finally
			{
				finalHadler?.Invoke();
				IsBusy = false;
			}
		}

		public async Task PushAsync<P>() where P : Page, new()
		{
			await RunAsync(async () => {
				await Page.Navigation.PushAsync(new P());
			});
		}

		public async Task PopAsync()
		{
			await RunAsync(async () => {
				await Page.Navigation.PopAsync();
			});
		}

	}
}
