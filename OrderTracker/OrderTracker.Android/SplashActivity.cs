using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace OrderTracker.Droid
{
	[Activity(Label = "Order Manager",
		 Icon = "@mipmap/ic_launcher",
		 RoundIcon = "@mipmap/ic_launcher",
		 Theme = "@style/MyTheme.Splash",
		 NoHistory = true,
		 MainLauncher = true,
		 ScreenOrientation = ScreenOrientation.Portrait)]
	public class SplashActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			InvokeMainActivity();
		}

		private void InvokeMainActivity()
		{
			StartActivity(new Intent(this, typeof(MainActivity)));
		}
	}
}