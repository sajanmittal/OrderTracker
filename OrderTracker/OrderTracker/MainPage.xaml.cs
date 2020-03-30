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

      protected override void OnAppearing()
      {
         AddAppTiles();
         base.OnAppearing();
      }

      private void AddAppTiles()
        {
         var apps = viewModel.GetApplications();
         BaseStackLayout.Children.Clear();
         int batchCount = 2;
         for(int i = 0; i < apps.Count; i += batchCount)
         {
            var count = batchCount;
            if (i + batchCount > apps.Count - 1)
               count = apps.Count - i;

            var currentApps = apps.GetRange(i, count).ToArray();
            BaseStackLayout.Children.Add(TileLayout(currentApps));
         }
        }

      private StackLayout TileLayout(params IPassApplication[] apps)
      {
         var parentLayout = new StackLayout
         {
            Orientation = StackOrientation.Horizontal,
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            BackgroundColor = Color.Transparent
         };

         foreach (var app in apps)
         {
            var layout = new StackLayout
            {
               Orientation = StackOrientation.Vertical,
               HeightRequest = 120,
               WidthRequest = 120,
               VerticalOptions = LayoutOptions.Fill,
               HorizontalOptions = LayoutOptions.Fill,
               BackgroundColor = Color.Transparent
            };

            var button = new ImageButton
            {
               Command = app.Command,
               Source = $"{app.Image}.png",
               VerticalOptions = LayoutOptions.CenterAndExpand,
               HorizontalOptions = LayoutOptions.CenterAndExpand,
               HeightRequest = 80,
               WidthRequest = 100,
               BorderColor = Color.Transparent,
               Aspect = Aspect.AspectFit
            };

            var label = new Label
            {
               FontSize = 18,
               FontAttributes = FontAttributes.Bold,
               Text = app.Name,
               VerticalOptions = LayoutOptions.EndAndExpand,
               HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            layout.Children.Add(button);
            layout.Children.Add(label);
            parentLayout.Children.Add(layout);
         }

         return parentLayout;
      }
    }
}
