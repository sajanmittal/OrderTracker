using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrderTracker
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainViewModel viewModel { get; set; }
        public MainPage()
        {
            InitializeComponent();
            if (viewModel == null)
            {
                viewModel = new MainViewModel(Navigation);
                BindingContext = viewModel;
            }
        }
    }
}
