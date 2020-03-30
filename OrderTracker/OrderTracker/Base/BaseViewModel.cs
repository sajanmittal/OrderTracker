using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
	}
}
