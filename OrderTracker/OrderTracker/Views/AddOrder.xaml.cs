using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrderTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrder : ContentPage
    {
        OrderViewModel viewModel;
        public AddOrder()
        {
            InitializeComponent();
            if (viewModel == null)
                viewModel = new OrderViewModel(Navigation, this);

            BindingContext = viewModel;
        }

        public void Save_Clicked(object sender, EventArgs e)
        {
            viewModel.SaveCommand.Execute(this);
        }
    }
}