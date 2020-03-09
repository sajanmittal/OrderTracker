using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrderTracker
{
    public abstract class BaseViewModel<T> : INotifyPropertyChanged where T : BaseModel, new()
    {
        public BaseViewModel(INavigation _navigation, Page page)
        {
            Navigation = _navigation;
            Page = page;
            if (model == null)
                ResetModel();
        }

        private bool isBusy;

        public bool IsBusy
        {
            get => isBusy;

            set
            {
                SetProperty(ref isBusy, value, nameof(IsBusy));
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

        private Page page;

        public Page Page
        {
            get => page;
            set
            {
                SetProperty(ref page, value, nameof(Page));
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
                if (!isBusy)
                {
                    IsBusy = true;
                    await action?.Invoke(data);
                }
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task RunAsync(Func<Task> action, Action<Exception> catchHandler = null, Action finalHadler = null)
        {
            try
            {
                if (!isBusy)
                {
                    IsBusy = true;
                    await action?.Invoke();
                }
            }
            catch (Exception ex)
            {
                catchHandler?.Invoke(ex);
                LoggerService.LogError(ex);
            }
            finally
            {
                finalHadler?.Invoke();
                IsBusy = false;
            }
        }
    }
}
