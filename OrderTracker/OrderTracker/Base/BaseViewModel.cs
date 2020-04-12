using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace OrderTracker
{
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		private bool isBusy;

		public bool IsBusy
		{
			get => isBusy;

			set => SetProperty(ref isBusy, value, nameof(IsBusy));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool SetProperty<D>(ref D storage, D value, [CallerMemberName]string propertyName = null)
		{
			if (EqualityComparer<D>.Default.Equals(storage, value))
				return false;

			storage = value;
			OnPropertyChanged(propertyName);
			return true;
		}

		public async Task RunAsync(Func<CancellationToken, Task> action, bool runInBackground = false, Action<Exception> catchHandler = null, Action finalHadler = null, CancellationTokenSource tokenSource = null)
		{
			try
			{
				if (!IsBusy)
				{
					IsBusy = true;
					if (tokenSource == null)
						tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(Constants.TASK_TIMEOUT));
					var task = runInBackground ? Task.Run(async () => { await action?.Invoke(tokenSource.Token); }, tokenSource.Token) : action?.Invoke(tokenSource.Token);
					await task;
				}
			}
			catch (Exception ex)
			{
				LoggerService.LogError(ex);
				catchHandler?.Invoke(ex);
				tokenSource.Cancel();
			}
			finally
			{
				finalHadler?.Invoke();
				IsBusy = false;
			}
		}
	}
}