﻿using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace OrderTracker
{
    public partial class App : Application
    {
        private static SqlLiteDbService db { get; set; }

        public static SqlLiteDbService DbService => db;
        public App()
        {
            InitializeComponent();
            Resources = StyleService.InitializeResources();
            MainPage = new NavigationPage(new MainPage());
            db = new SqlLiteDbService();
        }

        protected async override void OnStart()
        {
            AppDomain.CurrentDomain.UnhandledException += logError;
            await RequestPermissions();
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

        private async Task RequestPermissions()
        {
            if ((await Permissions.CheckStatusAsync<Permissions.StorageRead>()) == PermissionStatus.Denied)
            {
                await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            if ((await Permissions.CheckStatusAsync<Permissions.StorageWrite>()) == PermissionStatus.Denied)
            {
                await Permissions.RequestAsync<Permissions.StorageWrite>();
            }
        }

    }
}
