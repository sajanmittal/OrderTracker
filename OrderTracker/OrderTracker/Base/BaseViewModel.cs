using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace OrderTracker
{
    public abstract class BaseViewModel<T> : INotifyPropertyChanged where T:BaseModel, new()
    {
        public BaseViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            if(model == null)
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
    }
}
