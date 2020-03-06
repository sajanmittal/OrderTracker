using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrderTracker
{
    public abstract class BaseViewModel<T> : INotifyPropertyChanged where T : BaseModel, new()
    {
        public BaseViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            if (model == null)
                ResetModel();
        }

        private bool isNotBusy = true;

        public bool IsNotBusy
        {
            get => isNotBusy;

            set
            {
                SetProperty(ref isNotBusy, value, nameof(IsNotBusy));
            }
        }

        private T model;

        public T Model
        {
            get => model;
            set
            {
                SetProperty(ref model, value, nameof(Model));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<D>(ref D storage, D value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void ResetModel()
        {
            Model = new T();
        }

        public async Task RunAsync<D>(D data, Func<D, Task> action)
        {
            try
            {
                IsNotBusy = false;
                await action(data);
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex);
            }
            finally
            {
                IsNotBusy = true;
            }
        }

        public async Task RunAsync(Func<Task> action)
        {
            try
            {
                IsNotBusy = false;
                await action();
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex);
            }
            finally
            {
                IsNotBusy = true;
            }
        }
    }
}
