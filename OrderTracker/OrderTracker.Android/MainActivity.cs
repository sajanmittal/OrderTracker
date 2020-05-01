using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using EPlatform = Xamarin.Essentials.Platform;

namespace OrderTracker.Droid
{
	[Activity(Label = "Order Manager", Theme = "@style/MainTheme", Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			EPlatform.Init(this, savedInstanceState);
			Forms.Init(this, savedInstanceState);
			FormsMaterial.Init(this, savedInstanceState);
			CrossCurrentActivity.Current.Init(this, savedInstanceState);
			var startupFlags = Flags.AppStatupFlags.None;

			if (Intent?.Extras != null && !string.IsNullOrWhiteSpace(Intent.Extras.GetString(Constants.TITLE_KEY)))
			{
				startupFlags = Flags.AppStatupFlags.Notification;
			}

			LoadApplication(new App(startupFlags));
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			EPlatform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}