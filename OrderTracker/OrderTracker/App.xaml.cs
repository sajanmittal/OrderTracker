using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OrderTracker
{
	public partial class App : Application
	{
		private static SqlLiteDbService db { get; set; }

		public static SqlLiteDbService DbService => db;

		public App(Flags.AppStatupFlags statupFlags)
		{
			InitializeComponent();
			Resources = StyleService.InitializeResources();
			db = new SqlLiteDbService();
			DependencyService.Get<INotificationService>().Initialize();

			MainPage = new NavigationPage(new MainPage(statupFlags == Flags.AppStatupFlags.Notification));

			switch (statupFlags)
			{
				case Flags.AppStatupFlags.Notification:
					FireReceiveNotification();
					break;
			}
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

		private static void logError(object sender, UnhandledExceptionEventArgs args)
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

		private void FireReceiveNotification()
		{
			DependencyService.Get<INotificationService>().ReceiveNotification(null);
		}
	}
}