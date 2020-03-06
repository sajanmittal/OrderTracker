using System;
using Xamarin.Forms;

namespace OrderTracker
{
    public partial class App : Application
    {
        private static Lazy<SqlLiteDbService> db { get; set; }

        public static SqlLiteDbService DbService => db.Value;
        public App()
        {
            InitializeComponent();
            Resources = StyleService.InitializeResources();
            MainPage = new NavigationPage(new MainPage());
            db = new Lazy<SqlLiteDbService>();
        }

        protected override void OnStart()
        {
            AppDomain.CurrentDomain.UnhandledException += logError;
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        static void logError(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;
            LoggerService.LogError(ex);
        }
    }
}
