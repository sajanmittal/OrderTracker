
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using EPlatform = Xamarin.Essentials.Platform;

namespace OrderTracker.Droid
{
	[Activity(Label = "Order Manager", Theme = "@style/MainTheme", Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
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

			LoadApplication(new App());
		}
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			EPlatform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}