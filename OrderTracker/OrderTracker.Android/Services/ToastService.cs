using Android.Support.Design.Widget;
using Android.Widget;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AColor = Android.Graphics.Color;

[assembly: Dependency(typeof(OrderTracker.Droid.ToastService))]

namespace OrderTracker.Droid
{
	public class ToastService : IToastService
	{
		public void ShowSnackbar(string message)
		{
			var activity = CrossCurrentActivity.Current.Activity;
			Android.Views.View activityRootView = activity.FindViewById(Android.Resource.Id.Content);
			var bar = Snackbar.Make(activityRootView, message, Snackbar.LengthLong);
			var view = bar.View.FindViewById<TextView>(Resource.Id.snackbar_text);
			bar.View.SetBackgroundColor(AColor.OrangeRed);
			view.SetTextColor(AColor.White);
			bar.Show();
		}

		public void ShowToast(string message)
		{
			var toast = Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long);
			toast.View.SetBackgroundColor(Color.Accent.ToAndroid());
			toast.Show();
		}
	}
}