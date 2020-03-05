
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrderTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderList : ContentPage
    {
        public OrderViewModel viewModel;
        private SearchItem item;

        public OrderList(SearchItem searchItem)
        {
            InitializeComponent();
            item = searchItem;
            if (viewModel == null)
                viewModel = new OrderViewModel(Navigation);

            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {

            base.OnAppearing();
            if (item == null)
                return;

            viewModel.GetSearchDataCommand.Execute(item);
        }

        private void OrderListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Order;
            if(item != null)
            {
                viewModel.ItemTapped.Execute(item);
            }
        }
    }
}
