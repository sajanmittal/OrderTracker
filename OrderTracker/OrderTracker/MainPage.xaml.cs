using Xamarin.Forms;

namespace OrderTracker
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    public partial class MainPage : ContentPage
    {
        public MainViewModel viewModel { get; set; }
        public MainPage()
        {
            InitializeComponent();
            if (viewModel == null)
            {
                viewModel = new MainViewModel(this);
                BindingContext = viewModel;
            }
        }
    }
}
